using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils;
using SmgAlumni.Utils.Helpers;
using System;
using System.Linq;

namespace SmgAlumni.ServiceLayer
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool AddResetPassRequest(User user, Guid guid)
        {
            try
            {
                var request = new PasswordReset()
                {
                    DateCreated = DateTime.Now,
                    Used = false,
                    Guid = guid
                };

                user.PasswordResets.Add(request);
                _userRepository.Update(user);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void ChangePassword(string username, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password Cannot be empty");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username Cannot be empty");
            }

            var user = _userRepository.UsersByUserName(username).FirstOrDefault();
            if (user == null)
            {
                throw new NullReferenceException("Username Cannot be empty");
            }

            ChangePassword(user, password);
        }

        private void ChangePassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user is null");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password Cannot be empty");
            }

            var salt = Password.CreateSalt();
            user.PasswordSalt = salt;
            user.Password = Password.HashPassword(password + salt);
            _userRepository.Update(user);
        }

        public bool IsPasswordResetTokenValid(User user, Guid token)
        {
            return user.PasswordResets.Any(x => x.Guid == token && !x.Used);
        }

        public void ResetPasswordBasedOnToken(User user, Guid token, string password)
        {
            ChangePassword(user, password);
            InvalidatePasswordRequestToken(user, token);
        }

        private void InvalidatePasswordRequestToken(User user, Guid guid)
        {
            var passwordReset = user.PasswordResets.SingleOrDefault(x => x.Guid == guid);
            if (passwordReset == null)
            {
                throw new NullReferenceException("Password reset was not found");
            }

            passwordReset.Used = true;
            _userRepository.Update(user);
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
    }
}
