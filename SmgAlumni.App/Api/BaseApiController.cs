﻿using Microsoft.Owin.Security;
using Ninject;
using NLog;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class BaseApiController : ApiController
    {
        protected Logger _logger;

        public BaseApiController(Logger logger)
        {
            _logger = logger;
        }

        [Inject]
        public UserRepository Users { get; set; }

        private User currentUser;
        protected User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }

                if (currentUser == null)
                {
                    if (!AuthenticationManager.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                    {
                        return null;
                    }

                    var claim = AuthenticationManager.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
                    int id;
                    int.TryParse(claim.Value, out id);

                    currentUser = int.TryParse(claim.Value, out id) ? Users.GetById(id) : null;
                }

                return currentUser;
            }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        public void VerifyNotNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}