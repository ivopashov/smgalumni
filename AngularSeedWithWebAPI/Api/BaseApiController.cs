using Microsoft.Owin.Security;
using Mongo.Data;
using Mongo.Data.Entities;
using Ninject;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace AngularSeedWithWebAPI.Api
{
    public class BaseApiController : ApiController
    {
        protected Logger _logger;

        public BaseApiController(Logger logger)
        {
            _logger = logger;
        }

        [Inject]
        public MongoContext MongoSession { get; set; }

        private User _smUser;
        protected User SmUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }

                if (_smUser == null)
                {
                    if (!AuthenticationManager.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                    {
                        return null;
                    }

                    var claim = AuthenticationManager.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
                    _smUser = MongoSession.Users.GetById(claim.Value);
                }

                return _smUser;
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
