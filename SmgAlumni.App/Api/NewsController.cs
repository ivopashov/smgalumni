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
    public class NewsController : BaseApiController
    {
        private readonly NewsRepository _newsRepository;

        public NewsController(NewsRepository newsRepository, Logger logger) :base(logger)
        {
            _newsRepository = newsRepository;
            VerifyNotNull(_newsRepository);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/news/allnews")]
        public IHttpActionResult GetAllNews()
        {
            var news = _newsRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/news/count")]
        public IHttpActionResult GetNewsCount()
        {
            //var count = _newsRepository.GetAll().ToList().Count;
            return Ok(140);
        }

        [HttpPost]
        [Route("api/news/createnews")]
        public IHttpActionResult CreateNews(CauseNewsViewModelWithoutId vm)
        {
            var news = new News() { Body = vm.Body, Heading = vm.Heading, DateCreated = DateTime.Now, CreatedBy = User.Identity.Name };
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

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/news/newsbyid")]
        public IHttpActionResult GetNewsById([FromUri] int id)
        {
            var news = _newsRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterAdmin, User")]
        [Route("api/news/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _newsRepository.GetAll().Take(take).OrderBy(a=>a.DateCreated).Skip(skip).Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpPost]
        [Route("api/news/updatenews")]
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
    }
}
