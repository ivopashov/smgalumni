using SmgAlumni.App.Models;
using SmgAlumni.Utils.Identity;
using SmgAlumni.Utils.Membership;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class AuthenticationController : BaseApiController
    {
        private readonly UserManager _userManager;
        private readonly EFUserManager membership;

        public AuthenticationController(UserManager userManager, EFUserManager surveyMasterMembership, Logger logger)
            : base(logger)
        {
            _userManager = userManager;
            VerifyNotNull(_userManager);
            membership = surveyMasterMembership;
            VerifyNotNull(membership);
        }

        [AllowAnonymous]
        [Route("api/formsauthentication")]
        public IHttpActionResult FormsAuthentication(LoginViewModel login)
        {
            if (!ModelState.IsValid || (login == null))
            {
                return BadRequest("Грешни входни данни. Моля опитайте отново.");
            }

            var user = membership.GetUserByUserName(login.UserName);

            if (user == null || !membership.ValidatePassword(user, login.Password))
            {
                return BadRequest("Грешни входни данни. Моля опитайте отново.");
            }

            if (!user.Verified) return BadRequest("Вашият акаунт все още не е одобрен.");

            return Ok(GetToken(user.UserName, user.Email, user.Roles.Select(a => a.Name).ToList()));
        }

        private string CreateToken(string username)
        {
            var identity = _userManager.CreateIdentity(username, Startup.OAuthOptions.AuthenticationType);
            var ticket = _userManager.CreateAuthenticationTicket(identity);
            var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            return token;
        }

        private object GetToken(string username, string email, List<string> roles)
        {
            return new
            {
                Token = CreateToken(username),
                Username = username,
                Email = email,
                Roles = roles
            };
        }

        [AllowAnonymous]
        [Route("api/username")]
        public string GetUserName()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.UserName ?? "Unknown user";
            }
            return string.Empty;
        }
    }
}
