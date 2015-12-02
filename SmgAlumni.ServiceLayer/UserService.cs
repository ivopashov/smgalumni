using Microsoft.Owin.Security;
using RestSharp;
using RestSharp.Authenticators;
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
        private readonly IRequestSender _requestSender;

        public UserService(IAppSettings appSettings, IUserRepository userRepository, IRequestSender requestSender)
        {
            _appSettings = appSettings;
            _userRepository = userRepository;
            _requestSender = requestSender;
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
            user.AddedToNewsLetterList = false;

            _userRepository.Add(user);

            AddUserToNewsLetterList(user);
        }

        public bool AddUserToNewsLetterList(User user)
        {
            var result = SendRequest(user.Email, user.FirstName, user.LastName);
            var isSuccess = false;

            switch (result)
            {
                case System.Net.HttpStatusCode.OK:
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.Created:
                    isSuccess = true;
                    break;
            }

            if (isSuccess)
            {
                user.AddedToNewsLetterList = true;
                _userRepository.Update(user);
            }

            return isSuccess;
        }

        private System.Net.HttpStatusCode SendRequest(string email, string firstName, string lastName)
        {
            return _requestSender.InitializeClient()
                .AddParameter("address", email)
                .AddParameter("subscribed", true)
                .AddParameter("name", firstName + " " + lastName)
                .AddParameter("list", _appSettings.MailgunSettings.NewsLetterMailingList, ParameterType.UrlSegment)
                .SetResource("lists/{list}/members")
                .SetMethod(Method.POST)
                .Execute()
                .StatusCode;
        }
    }
}
