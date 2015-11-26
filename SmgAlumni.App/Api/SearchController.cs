using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using SmgAlumni.App.Logging;
using SmgAlumni.App.Models;
using SmgAlumni.Utils.Membership;

namespace SmgAlumni.App.Api
{
    public class SearchController : BaseApiController
    {
        private EFUserManager _userManager;

        public SearchController(ILogger logger, EFUserManager userManager)
            : base(logger)
        {
            _userManager = userManager;
            VerifyNotNull(_userManager);
        }

        [HttpGet]
        [Route("api/search/byid")]
        public IHttpActionResult GetUserById([FromUri]int id)
        {
            if (id == default(int)) return BadRequest("Невалидни входни данни");

            var user = Users.GetById(id);
            if (user == null) return BadRequest("Възникна грешка. Моля опитайте отново");

            var vm = Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }

        [HttpGet]
        [Route("api/search/short/byname")]
        public IHttpActionResult GetUserByNameShortVM([FromUri]string name)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            var users = Users.GetAll().Where(a => a.FirstName.ToLower().Contains(name.ToLower()) || a.MiddleName.ToLower().Contains(name.ToLower()) || a.LastName.ToLower().Contains(name.ToLower())).ToList();
            if (users.Any())
                foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);
            return Ok(foundUsers);
        }

        [HttpGet]
        [Route("api/search/short/byusername")]
        public IHttpActionResult GetUserByUserNameShortVM([FromUri]string username)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            var users = Users.GetAll().Where(a => a.UserName.ToLower().Contains(username.ToLower())).ToList();
            if (users.Any())
                foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);
            return Ok(foundUsers);
        }

        [HttpGet]
        [Route("api/search/long/byusernamecontains")]
        public IHttpActionResult GetUserByUserNameLongVM([FromUri]string username)
        {
            var users = Users.GetAll().Where(a => a.UserName.Contains(username)).ToList();
            var vm = Mapper.Map<List<UserAccountViewModel>>(users);
            return Ok(vm);
        }

        [HttpPost]
        [Route("api/search/bydivisionandyear")]
        public IHttpActionResult RerieveUsersByDivisionAndYear(SearchByDivisionAndYearViewModel vm)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var users = Users.GetAll().Where(a => a.Division == vm.Division && a.YearOfGraduation == vm.YearOfGraduation).ToList();
            if (users.Any()) foundUsers = Mapper.Map<List<UserAccountShortViewModel>>(users);

            return Ok(foundUsers);
        }
    }
}
