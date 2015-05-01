using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Security;
using SmgAlumni.App.Logging;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;

namespace SmgAlumni.App.Api
{
    [Authorize]
    public class BaseApiController : ApiController
    {
        protected ILogger _logger;

        public BaseApiController(ILogger logger)
        {
            _logger = logger;
        }

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
