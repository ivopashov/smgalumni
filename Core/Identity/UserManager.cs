﻿using Core.Membership;
using Core.Settings;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public class UserManager
    {
        private readonly AppSettings _appSettings;
        private readonly EFUserManager _membership;

        public UserManager(AppSettings appSettings, EFUserManager membership)
        {
            _appSettings = appSettings;
            _membership = membership;
        }

        public ClaimsIdentity CreateIdentity(string username, string authenticationType)
        {
            var identity = new ClaimsIdentity(authenticationType);
            var user = _membership.GetUserByUserName(username);
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
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
    }
}
