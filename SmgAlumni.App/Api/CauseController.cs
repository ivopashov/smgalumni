using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles = "Admin, MasterAdmin")]
    public class CauseController : BaseApiController
    {
        private readonly CauseRepository _causeRepository;

        public CauseController(CauseRepository causeRepository, Logger logger)
            : base(logger)
        {
            _causeRepository = causeRepository;
            VerifyNotNull(_causeRepository);
        }

        [HttpPost]
        [Route("api/cause/createcause")]
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
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/cause/causebyid")]
        public IHttpActionResult GetCauseById([FromUri] int id)
        {
            var news = _causeRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/cause/allcauses")]
        public IHttpActionResult GetAllCauses()
        {
            var news = _causeRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/cause/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _causeRepository.GetAll().Take(take).Skip(skip).Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/cause/count")]
        public IHttpActionResult GetCausesCount()
        {
            var count = _causeRepository.GetAll().ToList().Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/cause/updatecause")]
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
