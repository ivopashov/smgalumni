using AngularSeedWithWebAPI.Models;
using Core.Identity;
using Core.Membership;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularSeedWithWebAPI.Api
{
    public class AuthenticationController : BaseApiController
    {
        private readonly UserManager _userManager;
        private readonly MongoUserManager membership;

        public AuthenticationController(UserManager userManager, MongoUserManager surveyMasterMembership,Logger logger):base(logger)
        {
            _userManager = userManager;
            VerifyNotNull(_userManager);
            membership = surveyMasterMembership;
            VerifyNotNull(membership);
        }
        [Route("api/formsauthentication")]
        public IHttpActionResult FormsAuthentication(LoginViewModel login)
        {
            if (!ModelState.IsValid || (login == null))
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }
            var user = membership.GetUserByUserName(login.UserName);
            if (user == null || !membership.ValidatePassword(user, login.Password))
            {
                ModelState.AddModelError("error", "The user name or password is incorrect.");
                return BadRequest(ModelState);
            }
            return Ok(GetToken(user.UserName, user.Email));
        }

        private string CreateToken(string username)
        {
            var identity = _userManager.CreateIdentity(username, Startup.OAuthOptions.AuthenticationType); //TODO: isn't this always null
            var ticket = _userManager.CreateAuthenticationTicket(identity);
            var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            return token;
        }
        private object GetToken(string username, string email)
        {
            return new
            {
                Token = CreateToken(username),
                Username = username,
                Email = email
            };
        }

        [Route("api/username")]
        public string GetUserName()
        {
            if (SmUser != null)
            {
                return SmUser.UserName ?? "Unknown user";
            }
            return string.Empty;
        }
    }
}
