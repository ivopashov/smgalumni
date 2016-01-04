using AutoMapper;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class SearchController : BaseApiController
    {

        public SearchController(ILogger logger, IUserRepository userRepository)
            : base(logger, userRepository)
        {
        }

        [HttpGet]
        [Route("api/search/byid")]
        public IHttpActionResult GetUserById([FromUri]int id)
        {
            if (id == default(int))
            {
                return BadRequest("Невалидни входни данни");
            }

            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return BadRequest("Възникна грешка. Моля опитайте отново");
            }

            var vm = Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }

        [HttpGet]
        [Route("api/search/short/byname")]
        public IHttpActionResult GetUserByNameShortVM([FromUri]string name)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            var users = _userRepository.UsersByName(HttpContext.Current.Server.HtmlEncode(name));
            if (users.Any())
            {
                foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);
            }

            return Ok(foundUsers);
        }

        [HttpGet]
        [Route("api/search/short/byusername")]
        public IHttpActionResult GetUserByUserNameShortVM([FromUri]string username)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            var users = _userRepository.UsersByUserName(HttpContext.Current.Server.HtmlEncode(username));
            if (users.Any())
            {
                foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);
            }

            return Ok(foundUsers);
        }

        [HttpGet]
        [Route("api/search/long/byusernamecontains")]
        public IHttpActionResult GetUserByUserNameLongVM([FromUri]string username)
        {
            var users = _userRepository.UsersByUserName(HttpContext.Current.Server.HtmlEncode(username));
            var vm = Mapper.Map<List<UserAccountViewModel>>(users);
            return Ok(vm);
        }

        [HttpPost]
        [Route("api/search/bydivisionandyear")]
        public IHttpActionResult RerieveUsersByDivisionAndYear(SearchByDivisionAndYearViewModel vm)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни");
            }

            var users = _userRepository.UsersByDivisionAndYearOfGraduation(vm.Division, vm.YearOfGraduation);
            if (users.Any())
            {
                foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);
            }

            return Ok(foundUsers);
        }
    }
}
