using Microsoft.Owin.Security;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [Authorize]
    public class BaseApiController : ApiController
    {
        protected ILogger _logger;
        protected readonly IUserRepository _userRepository;

        public BaseApiController(ILogger logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
        }

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

                    currentUser = int.TryParse(claim.Value, out id) ? _userRepository.GetById(id) : null;
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
