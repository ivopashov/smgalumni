using SmgAlumni.Data.Interfaces;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class NewsletterController : BaseApiController
    {
        private readonly INewsLetterService _newsLetterService;

        public NewsletterController(ILogger logger, IUserRepository userRepository, INewsLetterService newsLetterService)
            : base(logger, userRepository)
        {
            _newsLetterService = newsLetterService;
        }

        [HttpGet]
        public IHttpActionResult Unsubscribe(string token)
        {
            _newsLetterService.Unsubscribe(token, CurrentUser.UserName);
            return Ok();
        }
    }
}
