﻿using NLog;
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
    public class ForumCommentController : BaseApiController
    {
        private readonly ForumAnswerRepository _forumAnswerRepository;
        private readonly ForumThreadRepository _forumThreadRepository;
        private readonly ForumCommentsRepository _forumCommentRepository;

        public ForumCommentController(ForumAnswerRepository forumAnswerRepository, Logger logger,
             ForumThreadRepository forumThreadRepository, ForumCommentsRepository forumCommentRepository)
            : base(logger)
        {
            _forumAnswerRepository = forumAnswerRepository;
            VerifyNotNull(_forumAnswerRepository);
            _forumThreadRepository = forumThreadRepository;
            VerifyNotNull(_forumThreadRepository);
            _forumCommentRepository = forumCommentRepository;
            VerifyNotNull(_forumCommentRepository);
        }

        [HttpPost]
        [Route("api/forumcomment/create")]
        public IHttpActionResult AddComment(ItemWithBodyAndParentIdViewModel vm)
        {
            var fa = _forumAnswerRepository.GetById(vm.ParentId);
            if (fa == null) return BadRequest("Отговорът, към който искахте да добавите коментара не беше намерен.");

            var comment = new ForumComment()
            {
                Body = vm.Body,
                CreatedOn = DateTime.Now,
                UserId = CurrentUser.Id
            };

            fa.Comments.Add(comment);
            _forumAnswerRepository.Update(fa);

            return Ok(new
            {
                Body = comment.Body,
                CreatedOn = comment.CreatedOn,
                CreatedBy = CurrentUser.UserName,
                Id = comment.Id,
                CanEdit = comment.UserId == CurrentUser.Id ? true : false,
            });
        }

        [HttpGet]
        [Route("api/forumcomment/forumcommentbyid")]
        public IHttpActionResult GetById([FromUri] int id)
        {
            var fa = _forumCommentRepository.GetById(id);
            return Ok(fa);
        }

        [HttpGet]
        [Route("api/forumcomment/delete")]
        public IHttpActionResult Delete(int id)
        {
            var fa = _forumCommentRepository.GetById(id);
            if (fa == null) return BadRequest("Темата не беше намерена. Моля опитайте отново.");
            try
            {
                _forumCommentRepository.Delete(fa);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Темата не може да бъде изтрита");
            }
        }

        [HttpPost]
        [Route("api/forumcomment/update")]
        public IHttpActionResult Update(AnswerBodyViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var fa = _forumCommentRepository.GetById(vm.Id);
            if (fa == null) return BadRequest("Темата с такова id не можа да бъде намерена");
            if (fa.User.Id != CurrentUser.Id)
                return BadRequest("Нямате права да променяте отговорът");
            fa.Body = vm.Body;
            fa.CreatedOn = DateTime.Now;

            try
            {
                _forumCommentRepository.Update(fa);
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