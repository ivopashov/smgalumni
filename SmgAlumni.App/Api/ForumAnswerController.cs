using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class ForumAnswerController : BaseApiController
    {
        private readonly ForumAnswerRepository _forumAnswerRepository;
        private readonly ForumThreadRepository _forumThreadRepository;

        public ForumAnswerController(ForumAnswerRepository forumAnswerRepository, Logger logger,
            ForumThreadRepository forumThreadRepository)
            : base(logger)
        {
            _forumAnswerRepository = forumAnswerRepository;
            VerifyNotNull(_forumAnswerRepository);
            _forumThreadRepository = forumThreadRepository;
            VerifyNotNull(_forumThreadRepository);
        }

        [HttpPost]
        [Route("api/forumanswer/create")]
        public IHttpActionResult Create(ItemWithBodyAndParentIdViewModel vm)
        {

            var fa = new ForumAnswer()
            {
                Body = vm.Body,
                CreatedOn = DateTime.Now,
                Likes = 0
            };

            try
            {
                var ft = _forumThreadRepository.GetById(vm.ParentId);
                ft.Answers.Add(fa);
                _forumThreadRepository.Update(ft);
                CurrentUser.ForumAnswers.Add(fa);
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
        [Route("api/forumanswer/forumanswerbyid")]
        public IHttpActionResult GetById([FromUri] int id)
        {
            var fa = _forumAnswerRepository.GetById(id);
            return Ok(fa);
        }

        [HttpGet]
        [Route("api/forumanswer/delete")]
        public IHttpActionResult Delete(int id)
        {
            var fa = _forumAnswerRepository.GetById(id);
            if (fa == null) return BadRequest("Темата не беше намерена. Моля опитайте отново.");
            try
            {
                _forumAnswerRepository.Delete(fa);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Темата не може да бъде изтрита");
            }
        }

        [HttpGet]
        [Route("api/forumanswer/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip, [FromUri]int forumthreadid)
        {
            var answers = _forumThreadRepository.GetById(forumthreadid).Answers.OrderBy(a => a.CreatedOn)
                .Take(take).Skip(skip)
                .Select(a => new { Answer = a, CanEdit = a.User.Id == CurrentUser.Id ? true : false }).ToList();
            return Ok(answers);
        }

        [HttpGet]
        [Route("api/forumanswer/count")]
        public IHttpActionResult GetCount([FromUri]int forumthreadid)
        {

            var count = _forumThreadRepository.GetById(forumthreadid).Answers.Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/forumanswer/update")]
        public IHttpActionResult Update(ForumAnswer vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var fa = _forumAnswerRepository.GetById(vm.Id);
            if (fa == null) return BadRequest("Темата с такова id не можа да бъде намерена");
            if(fa.User.Id!=CurrentUser.Id)
                return BadRequest("Нямате права да променяте отговорът");
            fa.Body = vm.Body;
            fa.CreatedOn = DateTime.Now;

            try
            {
                _forumAnswerRepository.Update(fa);
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