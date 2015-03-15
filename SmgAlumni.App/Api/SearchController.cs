using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class SearchController : BaseApiController
    {
        private UserRepository _userRepository;

        public SearchController(Logger logger, UserRepository userRepository):base(logger)
        {
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
        }

        [HttpGet]
        [Route("api/search/byid")]
        public IHttpActionResult GetUserById([FromUri]int id)
        {
            if (id == default(int)) return BadRequest("Невалидни входни данни");

            var user = _userRepository.GetById(id);
            if (user == null) return BadRequest("Възникна грешка. Моля опитайте отново");

            var vm = AutoMapper.Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }

        [HttpPost]
        [Route("api/search/bydivisionandyear")]
        public IHttpActionResult RerieveUsersByDivisionAndYear(SearchByDivisionAndYearViewModel vm)
        {
            var foundUsers = new List<UserAccountShortViewModel>();
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var users = _userRepository.GetAll().Where(a=>a.Division==vm.Division && a.YearOfGraduation==vm.YearOfGraduation).ToList();
            if (users.Any()) foundUsers = AutoMapper.Mapper.Map<List<UserAccountShortViewModel>>(users);
            
            return Ok(foundUsers);
        }
    }
}
