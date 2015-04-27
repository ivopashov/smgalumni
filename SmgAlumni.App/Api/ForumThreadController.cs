using System;
using System.Linq;
using System.Web.Http;
using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;

namespace SmgAlumni.App.Api
{
    public class ForumThreadController : BaseApiController
    {
        private readonly ForumThreadRepository _forumThreadRepository;

        public ForumThreadController(ForumThreadRepository forumThreadRepository, Logger logger)
            : base(logger)
        {
            _forumThreadRepository = forumThreadRepository;
            VerifyNotNull(_forumThreadRepository);
        }

        [HttpPost]
        [Route("api/forumthread/create")]
        public IHttpActionResult Create(CauseNewsViewModelWithoutId vm)
        {

            var ft = new ForumThread()
            {
                Body = vm.Body,
                Heading = vm.Heading,
                CreatedOn = DateTime.Now,
            };

            try
            {
                CurrentUser.ForumThreads.Add(ft);
                Users.Update(CurrentUser);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpGet]
        [Route("api/forumthread/forumthreadbyid")]
        public IHttpActionResult GetById([FromUri] int id)
        {
            var ft = _forumThreadRepository.GetById(id);
            if (ft == null) return BadRequest("Темата не можа да бъде намерена");

            return Ok(new { Body = ft.Body, Id = ft.Id, Heading = ft.Heading, CreatedOn = ft.CreatedOn, CreatedBy = ft.User.UserName });
        }

        [HttpGet]
        [Route("api/forumthread/allforumthreads")]
        public IHttpActionResult GetAll()
        {
            var ft = _forumThreadRepository.GetAll().
                Select(a => new { Heading = a.Heading, DateCreated = a.CreatedOn, Id = a.Id, CreatedBy = a.User.UserName }).ToList();
            return Ok(ft);
        }

        [HttpGet]
        [Route("api/forumthread/delete")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult Delete(int id)
        {
            var ft = _forumThreadRepository.GetById(id);
            if (ft == null) return BadRequest("Темата не беше намерена. Моля опитайте отново.");
            try
            {
                _forumThreadRepository.Delete(ft);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Темата не може да бъде изтрита");
            }
        }

        [HttpGet]
        [Route("api/forumthread/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _forumThreadRepository.GetAll().OrderBy(a => a.CreatedOn).Take(take).Skip(skip).
                Select(a => new { Heading = a.Heading, DateCreated = a.CreatedOn, Id = a.Id, CreatedBy = a.User.UserName, NumberOfAnswers = a.Answers.Count }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/forumthread/count")]
        public IHttpActionResult GetCount()
        {
            var count = _forumThreadRepository.GetAll().ToList().Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/forumthread/update")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult Update(ForumThread vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var ft = _forumThreadRepository.GetById(vm.Id);
            if (ft == null) return BadRequest("Темата с такова id не можа да бъде намерена");
            ft.Body = vm.Body;
            ft.Heading = vm.Heading;
            ft.CreatedOn = DateTime.Now;

            try
            {
                _forumThreadRepository.Update(ft);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Темата не можа да бъде променена");
            }

        }
    }
}