using NLog;
using SmgAlumni.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.Helpers;
using System.Security.Claims;
using SmgAlumni.EF.Models.enums;

namespace SmgAlumni.Utils.Membership
{
    public class EFUserManager
    {
        private readonly UserRepository _userRepository;
        private readonly Logger _logger;

        public EFUserManager(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public User GetUserByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            var result=_userRepository.Find(a => a.UserName.Equals(username)).ToList();
            if (!result.Any()) return null;
            return result[0];
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return _userRepository.Find(x => x.Email.Equals(email)).SingleOrDefault();
        }

        public bool AddResetPassRequest(User user, Guid guid)
        {
            try
            {
                var request = new PasswordReset()
                {
                    DateCreated = DateTime.Now,
                    Used = false,
                    Guid=guid
                };

                user.PasswordResets.Add(request);
                _userRepository.Update(user);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Couldn't add reset password request for user "+user.UserName);
                return false;
            }
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public List<string> GetRoles(User user)
        {
            return user.Roles.Select(a => a.Name).ToList();
        }

        public void CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentNullException("username is required field");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentNullException("password is required field");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException("email is required field");
            }
            //Make sure that email is not already registered

            if (GetUserByEmail(user.Email) != null)
            {
                throw new Exception("That email address has already been registered");
            }

            if (GetUserByUserName(user.UserName) != null)
            {
                throw new Exception("That username has already been registered");
            }

            //create the new user
            var salt = Password.CreateSalt();
            user.Password = Password.HashPassword(user.Password + salt);
            user.PasswordSalt = salt;
            user.DateJoined = DateTime.Now;
            user.Roles.Add(new Role() {Name="User"});
            user.Verified = false;

            try
            {
                _userRepository.Add(user);
            }
            catch (Exception e)
            {
                _logger.Error("Couldn't add " + user.UserName + " to the database");
            }
        }

        public bool ValidatePassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user is null");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password is null");
            }

            return user.Password == (Password.HashPassword(password + user.PasswordSalt));
        }

        public void ChangePassword(string username, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password Cannot be empty");
            }

            var user = GetUserByUserName(username);
            ChangePassword(user, password);
        }

        public void ResetPasswordBasedOnToken(User user, Guid token, string password)
        {
            ChangePassword(user, password);
            InvalidatePasswordRequestToken(user, token);
        }

        private void ChangePassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user is null");
            }

            var salt = Password.CreateSalt();
            user.PasswordSalt = salt;
            user.Password = Password.HashPassword(password + salt);
            _userRepository.Update(user);
        }

        private void InvalidatePasswordRequestToken(User user, Guid guid)
        {
            var index = user.PasswordResets.SingleOrDefault(x => x.Guid==guid);
            if (index == null) throw new Exception();
            index.Used = true;
            _userRepository.Update(user);
        }

        public bool IsPasswordResetTokenValid(User user, Guid token)
        {
            return user.PasswordResets.Any(x => x.Guid == token && !x.Used);
        }
    }
}
