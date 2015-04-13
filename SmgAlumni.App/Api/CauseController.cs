using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class CauseController : BaseApiController
    {
        private readonly CauseRepository _causeRepository;
        private readonly EFUserManager _userManager;
        private User _user;

        public CauseController(CauseRepository causeRepository, Logger logger, EFUserManager userManager)
            : base(logger)
        {
            _causeRepository = causeRepository;
            VerifyNotNull(_causeRepository);
            _userManager = userManager;
            VerifyNotNull(_userManager);
            _user = _userManager.GetUserByUserName(User.Identity.Name);
        }

        [HttpPost]
        [Route("api/cause/createcause")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult CreateCause(CauseNewsViewModelWithoutId vm)
        {
            var cause = new Cause()
            {
                Body = vm.Body,
                Heading = vm.Heading,
                DateCreated = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Enabled = true,
                UserId = _user == null ? 0 : _user.Id
            };

            try
            {
                _causeRepository.Add(cause);
                DomainEvents.Raise<AddCauseDomainEvent>(new AddCauseDomainEvent() { Heading = cause.Heading, UserId = _user == null ? 0 : _user.Id });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpGet]
        [Route("api/cause/causebyid")]
        public IHttpActionResult GetCauseById([FromUri] int id)
        {
            var news = _causeRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/cause/allcauses")]
        public IHttpActionResult GetAllCauses()
        {
            var news = _causeRepository.GetAll().
                Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/cause/delete")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult DeleteCause(int id)
        {
            var user = _userManager.GetUserByUserName(User.Identity.Name);
            var listings = _causeRepository.GetById(id);
            if (listings == null) return BadRequest("Каузата не беше намерена. Моля опитайте отново.");
            try
            {
                _causeRepository.Delete(listings);
                DomainEvents.Raise<DeleteCauseDomainEvent>(new DeleteCauseDomainEvent() { Heading = listings.Heading, UserId = _user == null ? 0 : _user.Id });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Каузата не може да бъде изтрита");
            }
        }

        [HttpGet]
        [Route("api/cause/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _causeRepository.GetAll().OrderBy(a => a.DateCreated).Take(take).Skip(skip).
                Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, CreatedBy = a.CreatedBy, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/cause/count")]
        public IHttpActionResult GetCausesCount()
        {
            var count = _causeRepository.GetAll().ToList().Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/cause/updatecause")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult UpdateCause(Cause vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var cause = _causeRepository.GetById(vm.Id);
            if (cause == null) return BadRequest("Новина с такова id не можа да бъде намерена");
            cause.Body = vm.Body;
            cause.Heading = vm.Heading;
            cause.DateCreated = DateTime.Now;
            cause.CreatedBy = User.Identity.Name;
            try
            {
                _causeRepository.Update(cause);
                DomainEvents.Raise<ModifyCauseDomainEvent>(new ModifyCauseDomainEvent() { Heading = cause.Heading, UserId = _user == null ? 0 : _user.Id });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Каузата не можа да бъде променена");
            }

        }
    }
}
