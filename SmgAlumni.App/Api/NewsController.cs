using System;
using System.Linq;
using System.Web.Http;
using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;

namespace SmgAlumni.App.Api
{
    public class NewsController : BaseApiController
    {
        private readonly NewsRepository _newsRepository;

        public NewsController(NewsRepository newsRepository, Logger logger)
            : base(logger)
        {
            _newsRepository = newsRepository;
            VerifyNotNull(_newsRepository);
          
        }

        [HttpGet]
        [Route("api/news/allnews")]
        public IHttpActionResult GetAllNews()
        {
            var news = _newsRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/news/count")]
        public IHttpActionResult GetNewsCount()
        {
            var count = _newsRepository.GetAll().ToList().Count;
            return Ok(count);
        }


        [HttpGet]
        [Route("api/news/delete")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult DeleteNews(int id)
        {
            var listings = _newsRepository.GetById(id);
            if (listings == null) return BadRequest("Новината не беше намерена. Моля опитайте отново.");
            try
            {
                _newsRepository.Delete(listings);
                DomainEvents.Raise<DeleteNewsDomainEvent>(new DeleteNewsDomainEvent() { Heading = listings.Heading, User = CurrentUser });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде изтрита");
            }

        }

        [HttpPost]
        [Route("api/news/createnews")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult CreateNews(CauseNewsViewModelWithoutId vm)
        {
            var news = new News()
            {
                Body = vm.Body,
                Heading = vm.Heading,
                DateCreated = DateTime.Now,
                Enabled = true,
            };

            try
            {
                CurrentUser.News.Add(news);
                Users.Update(CurrentUser);
                DomainEvents.Raise<AddNewsDomainEvent>(new AddNewsDomainEvent() { Heading = news.Heading, User = CurrentUser });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpGet]
        [Route("api/news/newsbyid")]
        public IHttpActionResult GetNewsById([FromUri] int id)
        {
            var news = _newsRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/news/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _newsRepository.GetAll().Take(take).OrderBy(a => a.DateCreated).Skip(skip)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, CreatedBy = a.User.UserName, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpPost]
        [Route("api/news/updatenews")]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public IHttpActionResult UpdateNews(News vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var news = _newsRepository.GetById(vm.Id);
            if (news == null) return BadRequest("Новина с такова id не можа да бъде намерена");
            news.Body = vm.Body;
            news.Heading = vm.Heading;
            news.DateCreated = DateTime.Now;

            try
            {
                _newsRepository.Update(news);
                DomainEvents.Raise<ModifyNewsDomainEvent>(new ModifyNewsDomainEvent() { Heading = news.Heading, User = CurrentUser });
                return Ok();

            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде променена");
            }
        }
    }
}
