﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using System.Web;

namespace SmgAlumni.App.Api
{
    public class ForumThreadController : BaseApiController
    {
        private readonly ForumThreadRepository _forumThreadRepository;

        public ForumThreadController(ForumThreadRepository forumThreadRepository, ILogger logger, IUserRepository userRepository)
            : base(logger, userRepository)
        {
            _forumThreadRepository = forumThreadRepository;
            VerifyNotNull(_forumThreadRepository);
        }

        [HttpPost]
        [Route("api/forumthread/create")]
        public IHttpActionResult Create(ForumThreadViewModel vm)
        {

            var ft = new ForumThread()
            {
                Body = HttpContext.Current.Server.HtmlEncode(vm.Body),
                Heading = HttpContext.Current.Server.HtmlEncode(vm.Heading),
                Tags = AutoMapper.Mapper.Map<List<Tag>>(vm.Tags),
                CreatedOn = DateTime.Now,
            };

            try
            {
                CurrentUser.ForumThreads.Add(ft);
                _userRepository.Update(CurrentUser);
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
        [Route("api/forumthread/forumthreadbyid")]
        public IHttpActionResult GetById([FromUri] int id)
        {
            var ft = _forumThreadRepository.GetById(id);
            if (ft == null) 
            { 
                return BadRequest("Темата не можа да бъде намерена"); 
            }

            return Ok(new { Body = ft.Body, Id = ft.Id, Heading = ft.Heading, CreatedOn = ft.CreatedOn, CreatedBy = ft.User.UserName, Tags = ft.Tags });
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

        [AllowAnonymous]
        [HttpGet]
        [Route("api/forumthread/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _forumThreadRepository.Page(skip, take)
                .Select(a => new { Heading = a.Heading, DateCreated = a.CreatedOn, Id = a.Id, CreatedBy = a.User.UserName, NumberOfAnswers = a.Answers.Count}).ToList();
            return Ok(news);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/forumthread/count")]
        public IHttpActionResult GetCount()
        {
            var count = _forumThreadRepository.GetCount();
            return Ok(count);
        }

        [HttpPost]
        [Route("api/forumthread/update")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult Update(ForumThread vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни");
            }

            var ft = _forumThreadRepository.GetById(vm.Id);
            if (ft == null)
            {
                return BadRequest("Темата с такова id не можа да бъде намерена");
            }

            ft.Body = HttpContext.Current.Server.HtmlEncode(vm.Body);
            ft.Heading = HttpContext.Current.Server.HtmlEncode(vm.Heading);
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