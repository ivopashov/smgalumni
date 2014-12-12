using Core.Helpers;
using Mongo.Data;
using Mongo.Data.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Membership
{
    public class MongoUserManager
    {
        private readonly MongoContext _dataContext;
        private readonly Logger _logger;

        public MongoUserManager(MongoContext dataContext)
        {
            _dataContext = dataContext;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public User GetUserByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            return _dataContext.Users.SingleOrDefault(x => x.UserName.Equals(username));
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return _dataContext.Users.SingleOrDefault(x => x.Email.Equals(email));
        }

        public bool AddResetPassRequest(User user, Guid guid)
        {
            try
            {
                var request = new PasswordReset()
                {
                    Guid = guid,
                    DateCreated = DateTime.Now,
                    Used = false,
                };
                if (user.PasswordResets == null)
                {
                    user.PasswordResets = new List<PasswordReset>();
                }
                user.PasswordResets.Add(request);
                _dataContext.Users.Update(user);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Couldn't add reset password request");
                return false;
            }
        }

        public User GetUserById(string id)
        {
            return _dataContext.Users.GetById(id);
        }

        public List<string> GetRoles(User user)
        {
            return user.Roles;
        }

        public void CreateUser(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username is required field");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password is required field");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email is required field");
            }
            //Make sure that email is not already registered

            if (GetUserByEmail(email) != null)
            {
                throw new Exception("That email address has already been registered");
            }

            if (GetUserByUserName(username) != null)
            {
                throw new Exception("That username has already been registered");
            }

            //create the new user
            var user = new User();
            var salt = Password.CreateSalt();
            user.Email = email;
            user.Password = Password.HashPassword(password + salt);
            user.PasswordSalt = salt;
            user.DateCreated = DateTime.Now;
            user.UserName = username;

            try
            {
                _dataContext.Users.Add(user);
            }
            catch (Exception e)
            {
                _logger.Error("Couldn't add " + username + " to the database");
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

        public void ResetPasswordBasedOnToken(User user, PasswordReset token, string password)
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
            _dataContext.Users.Update(user);
        }

        private void InvalidatePasswordRequestToken(User user, PasswordReset record)
        {
            var index = user.PasswordResets.FindIndex(x => x.Guid == record.Guid);
            user.PasswordResets[index].DateUsed = DateTime.Now;
            user.PasswordResets[index].Used = true;
            _dataContext.Users.Update(user);
        }

        public bool IsPasswordResetTokenValid(User user, Guid token)
        {
            return user.PasswordResets.Any(x => x.Guid == token && !x.Used);
        }

        public Task<User> GetByCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) 
            { 
                throw new ArgumentNullException("username cannot be null"); 
            }

            if (string.IsNullOrEmpty(password)) 
            {
                throw new ArgumentNullException("password cannot be null"); 
            }

            var user = GetUserByUserName(username);
            if (user == null)
            {
                return Task.FromResult<User>(null);
            }
            return ValidatePassword(user, password) ? Task.FromResult(user) : Task.FromResult<User>(null);
        }

        public Task<ClaimsIdentity> CreateIdentityAsync(User user, string applicationCookie)
        {
            if (user == null) 
            { 
                throw new ArgumentNullException("user cannot be null"); 
            }

            if (string.IsNullOrEmpty(applicationCookie)) 
            { 
                throw new ArgumentNullException("applicationCookie"); 
            }

            var claimsIdentity = new ClaimsIdentity(applicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            AddDefaultClaims(user, claimsIdentity);

            foreach (var role in GetRoles(user))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
            }

            return Task.FromResult(claimsIdentity);
        }

        private static void AddDefaultClaims(User user, ClaimsIdentity claimsIdentity)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(),
                "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
        }

      
    }
}
