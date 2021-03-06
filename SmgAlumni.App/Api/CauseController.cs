﻿using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [AllowAnonymous]
    public class CauseController : BaseApiController
    {
        private readonly ICauseRepository _causeRepository;

        public CauseController(ICauseRepository causeRepository, ILogger logger, IUserRepository userRepository)
            : base(logger, userRepository)
        {
            _causeRepository = causeRepository;
            VerifyNotNull(_causeRepository);
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
                LastModified = DateTime.Now,
                Enabled = true
            };

            try
            {
                CurrentUser.Causes.Add(cause);
                _userRepository.Update(CurrentUser);
                DomainEvents.Raise(new AddCauseDomainEvent() { Heading = vm.Heading, User = CurrentUser, Body = vm.Body });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Каузата не можа да бъде създадена");
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
        [Route("api/cause/delete")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult DeleteCause(int id)
        {
            var listings = _causeRepository.GetById(id);
            if (listings == null) return BadRequest("Каузата не беше намерена. Моля опитайте отново.");
            try
            {
                _causeRepository.Delete(listings);
                DomainEvents.Raise(new DeleteCauseDomainEvent() { Heading = listings.Heading, User = CurrentUser });
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
            var news = _causeRepository.Page(skip, take)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, CreatedBy = a.User.UserName, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/cause/count")]
        public IHttpActionResult GetCausesCount()
        {
            var count = _causeRepository.GetCount();
            return Ok(count);
        }

        [HttpPost]
        [Route("api/cause/updatecause")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult UpdateCause(Cause vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни");
            }

            var cause = _causeRepository.GetById(vm.Id);
            if (cause == null)
            {
                return BadRequest("Кауза с такова id не можа да бъде намерена");
            }

            cause.Body = vm.Body;
            cause.Heading = vm.Heading;
            cause.LastModified = DateTime.Now;

            try
            {
                _causeRepository.Update(cause);
                DomainEvents.Raise(new ModifyCauseDomainEvent() { Heading = cause.Heading, User = CurrentUser });
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
