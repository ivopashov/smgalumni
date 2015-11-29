using Microsoft.Owin.Security;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils.Helpers;
using SmgAlumni.Utils.Settings;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmgAlumni.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly IAppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UserService(IAppSettings appSettings, IUserRepository userRepository)
        {
            _appSettings = appSettings;
            _userRepository = userRepository;
        }

        public ClaimsIdentity CreateIdentity(string username, string authenticationType)
        {
            var identity = new ClaimsIdentity(authenticationType);
            var user = _userRepository.UsersByUserName(username).FirstOrDefault();
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            identity.AddClaim(new Claim("Verified", user.Verified ? "true" : "false"));
            return identity;
        }

        public AuthenticationTicket CreateAuthenticationTicket(ClaimsIdentity identity)
        {
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

            var currentUtc = DateTime.UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(_appSettings.Authentication.TokenExpirationLength);

            return ticket;
        }

        public void CreateUser(User user, bool receiveNewsletter)
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

            if (_userRepository.UsersByEmail(user.Email).SingleOrDefault() != null)
            {
                throw new Exception("That email address has already been registered");
            }

            if (_userRepository.UsersByUserName(user.UserName).SingleOrDefault() != null)
            {
                throw new Exception("That username has already been registered");
            }

            //create the new user
            var salt = Password.CreateSalt();
            user.Password = Password.HashPassword(user.Password + salt);
            user.PasswordSalt = salt;
            user.DateJoined = DateTime.Now;
            user.Roles.Add(new Role() { Name = "User" });
            user.Verified = false;

            if (receiveNewsletter)
            {
                var newsletterNoty = new NotificationSubscription()
                {
                    CreatedOn = DateTime.Now,
                    NotificationKind = EF.Models.enums.NotificationKind.NewsLetter,
                    UnsubscribeToken = Guid.NewGuid(),
                    Enabled = true
                };
                user.NotificationSubscriptions.Add(newsletterNoty);
            }

            _userRepository.Add(user);
        }

        public void UpdateUserNewsLetterNotification(bool subscribe, string username)
        {
            var user = _userRepository.UsersByUserName(username).FirstOrDefault();
            if (user == null)
            {
                throw new NullReferenceException();
            }

            var newsLetterSubscription = user.NotificationSubscriptions.Where(a => a.NotificationKind == EF.Models.enums.NotificationKind.NewsLetter).SingleOrDefault();

            //if we dont have any newsletter subscriptions but want to subscribe for the first time
            if (newsLetterSubscription == null && subscribe)
            {
                var noty = new NotificationSubscription()
                {
                    CreatedOn = DateTime.Now,
                    NotificationKind = EF.Models.enums.NotificationKind.NewsLetter,
                    UnsubscribeToken = Guid.NewGuid(),
                    Enabled = true
                };

                user.NotificationSubscriptions.Add(noty);
            }

            if (newsLetterSubscription != null)
            {
                if (subscribe && subscribe != newsLetterSubscription.Enabled)
                {
                    newsLetterSubscription.Enabled = true;
                    newsLetterSubscription.UnsubscribeToken = Guid.NewGuid();
                }
                if (!subscribe && subscribe != newsLetterSubscription.Enabled)
                {
                    newsLetterSubscription.Enabled = false;
                }
            }

            _userRepository.Update(user);
        }
    }
}
