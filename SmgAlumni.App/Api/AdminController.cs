using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles="Admin, MasterAdmin")]
    public class AdminController : BaseApiController
    {

        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly NewsRepository _newsRepository;
        private readonly CauseRepository _causeRepository;

        public AdminController(Logger logger, UserRepository userRepository, RoleRepository roleRepository, NewsRepository newsRepository, CauseRepository causeRepository):base(logger)
        {
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
            _roleRepository = roleRepository;
            VerifyNotNull(_roleRepository);
            _newsRepository = newsRepository;
            VerifyNotNull(_newsRepository);
            _causeRepository = causeRepository;
            VerifyNotNull(_causeRepository);
        }

        [HttpGet]
        [Route("api/admin/unverifiedusers")]
        public IHttpActionResult GetUnverifiedUsers()
        {
            var vm = new List<UserForVerifyViewModel>();
            try
            {
                var unverifiedUsers = _userRepository.GetAll().Where(a => !a.Verified).ToList();
                if (unverifiedUsers.Any())
                {
                    vm = AutoMapper.Mapper.Map<List<UserForVerifyViewModel>>(unverifiedUsers);
                }
                return Ok(vm);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Възникна грешка с колекцията с неверифицирани потребители. Моля опитайте отново");
            }

        }

        [HttpPost]
        [Route("api/admin/verifyusers")]
        public IHttpActionResult VerifyUsers(UserIdsToVerifyViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");
            if (vm.IdsToVerify.Count == 0) return Ok();
            //put everything in a transaction so that if anything fails nothing goes through
            using (var context = new SmgAlumniContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in vm.IdsToVerify)
                        {
                            var user = context.Users.SingleOrDefault(a => a.Id == item);
                            if (user == null) throw new Exception("Tried to verify user with id: "+item+" but user does not exist");
                            user.Verified = true;
                        }
                        context.SaveChanges();

                        transaction.Commit();

                        return Ok();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message);
                        transaction.Rollback();
                        return BadRequest("Възникна грешка и операцията не беше изпълнена. Моля опитайте отново");
                    }
                }
            }
        }

        [HttpGet]
        [Route("api/admin/allnews")]
        public IHttpActionResult GetAllNews()
        {
            var news = _newsRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id=a.Id }).ToList();
            return Ok(news);
        }

        [HttpPost]
        [Route("api/admin/createnews")]
        public IHttpActionResult CreateNews(CauseNewsViewModelWithoutId vm)
        {
            var news = new News() {Body=vm.Body, Heading=vm.Heading, DateCreated=DateTime.Now,CreatedBy=User.Identity.Name};
            try
            {
                _newsRepository.Add(news);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpPost]
        [Route("api/admin/createcause")]
        public IHttpActionResult CreateCause(CauseNewsViewModelWithoutId vm)
        {
            var cause = new Cause() { Body = vm.Body, Heading = vm.Heading, DateCreated = DateTime.Now, CreatedBy = User.Identity.Name };
            try
            {
                _causeRepository.Add(cause);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpGet]
        [Route("api/admin/newsbyid")]
        public IHttpActionResult GetNewsById([FromUri] int id)
        {
            var news = _newsRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/admin/causebyid")]
        public IHttpActionResult GetCauseById([FromUri] int id)
        {
            var news = _causeRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/admin/allcauses")]
        public IHttpActionResult GetAllCauses()
        {
            var news = _causeRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id=a.Id }).ToList();
            return Ok(news);
        }

        [HttpPost]
        [Route("api/admin/updatenews")]
        public IHttpActionResult UpdateNews(News vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var news = _newsRepository.GetById(vm.Id);
            if (news == null) return BadRequest("Новина с такова id не можа да бъде намерена");
            news.Body = vm.Body;
            news.Heading = vm.Heading;
            news.DateCreated = DateTime.Now;
            news.CreatedBy = User.Identity.Name;
            _newsRepository.Update(news);

            return Ok();
        }

        [HttpPost]
        [Route("api/admin/updatecause")]
        public IHttpActionResult UpdateCause(Cause vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var cause = _causeRepository.GetById(vm.Id);
            if (cause == null) return BadRequest("Новина с такова id не можа да бъде намерена");
            cause.Body = vm.Body;
            cause.Heading = vm.Heading;
            cause.DateCreated = DateTime.Now;
            cause.CreatedBy = User.Identity.Name;
            _causeRepository.Update(cause);

            return Ok();
        }



    }
}
