﻿using SmgAlumni.App.Logging;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class ForumAnswerController : BaseApiController
    {
        private readonly ForumAnswerRepository _forumAnswerRepository;
        private readonly ForumThreadRepository _forumThreadRepository;
        private readonly ForumCommentsRepository _forumCommentsRepositoty;

        public ForumAnswerController(ForumAnswerRepository forumAnswerRepository, ILogger logger,
            ForumThreadRepository forumThreadRepository, ForumCommentsRepository forumCommentsRepositoty, IUserRepository userRepository)
            : base(logger, userRepository)
        {
            _forumAnswerRepository = forumAnswerRepository;
            VerifyNotNull(_forumAnswerRepository);
            _forumThreadRepository = forumThreadRepository;
            VerifyNotNull(_forumThreadRepository);
            _forumCommentsRepositoty = forumCommentsRepositoty;
            VerifyNotNull(_forumCommentsRepositoty);
        }

        [HttpPost]
        [Route("api/forumanswer/create")]
        public IHttpActionResult Create(ItemWithBodyAndParentIdViewModel vm)
        {

            var fa = new ForumAnswer()
            {
                Body = HttpContext.Current.Server.HtmlEncode(vm.Body),
                CreatedOn = DateTime.Now,
                Likes = 0,
                UserId = CurrentUser.Id
            };

            try
            {

                var ft = _forumThreadRepository.GetById(vm.ParentId);
                ft.Answers.Add(fa);
                _forumThreadRepository.Update(ft);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [AllowAnonymous]
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
            if (fa == null)
            {
                return BadRequest("Темата не беше намерена. Моля опитайте отново.");
            }

            var comments = _forumCommentsRepositoty.GetCommentsForAnswer(fa.Id);
            try
            {
                foreach (var item in comments)
                {
                    _forumCommentsRepositoty.Delete(item);
                }

                _forumAnswerRepository.Delete(fa);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Темата не може да бъде изтрита");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/forumanswer/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip, [FromUri]int forumthreadid)
        {
            Func<int, bool> canEdit = (x) =>
             {
                 if (CurrentUser == null)
                 {
                     return false;
                 }
                 return CurrentUser.Id == x;
             };
            var answers = _forumThreadRepository.GetById(forumthreadid).Answers.OrderBy(a => a.CreatedOn)
                .Take(take).Skip(skip);
            var flattenedAnswers = answers.Select(a => new
            {
                Body = a.Body,
                CreatedOn = a.CreatedOn,
                CreatedBy = a.User.UserName,
                Id = a.Id,
                Likes = a.Likes,
                //CanEdit = a.User.Id == CurrentUser.Id ? true : false,
                CanEdit = canEdit(a.UserId),
                Comments = a.Comments.Select(b => new
                {
                    Body = b.Body,
                    CreatedOn = b.CreatedOn,
                    CreatedBy = b.User.UserName,
                    Id = b.Id,
                    CanEdit = canEdit(a.UserId)
                })
            })
                .ToList();

            return Ok(flattenedAnswers);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/forumanswer/count")]
        public IHttpActionResult GetCount([FromUri]int forumthreadid)
        {

            var count = _forumThreadRepository.GetById(forumthreadid).Answers.Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/forumanswer/update")]
        public IHttpActionResult Update(AnswerBodyViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var fa = _forumAnswerRepository.GetById(vm.Id);
            if (fa == null) return BadRequest("Темата с такова id не можа да бъде намерена");
            if (fa.User.Id != CurrentUser.Id)
                return Unauthorized();
            fa.Body = HttpContext.Current.Server.HtmlEncode(vm.Body);
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

        [HttpGet]
        [Route("api/forumanswer/modifylikescount")]
        public IHttpActionResult ModifyLikesCount([FromUri] int Id, [FromUri] int Like)
        {
            var answer = _forumAnswerRepository.GetById(Id);
            answer.Likes += Like;
            _forumAnswerRepository.Update(answer);
            return Ok();
        }
    }
}