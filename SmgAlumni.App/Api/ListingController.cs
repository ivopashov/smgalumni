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
    public class ListingController : BaseApiController
    {
        private readonly ListingRepository _listingRepository;

        public ListingController(ListingRepository listingRepository, Logger logger)
            : base(logger)
        {
            _listingRepository = listingRepository;
            VerifyNotNull(_listingRepository);
        }

        [HttpPost]
        [Route("api/listing/createlisting")]
        public IHttpActionResult CreateListing(CauseNewsViewModelWithoutId vm)
        {
            var listing = new Listing() 
            { 
                Body = vm.Body, 
                Heading = vm.Heading, 
                DateCreated = DateTime.Now, 
                CreatedBy = User.Identity.Name,
                Enabled=true
            };

            try
            {
                _listingRepository.Add(listing);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Новината не можа да бъде създадена");
            }
        }

        [HttpGet]
        [Route("api/listing/listingbyid")]
        public IHttpActionResult GetListingById([FromUri] int id)
        {
            var news = _listingRepository.GetById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/alllistings")]
        public IHttpActionResult GetAllListings()
        {
            var news = _listingRepository.GetAll().Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _listingRepository.GetAll().Take(take).Skip(skip).Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/count")]
        public IHttpActionResult GetListingsCount()
        {
            var count = _listingRepository.GetAll().ToList().Count;
            return Ok(count);
        }

        [HttpPost]
        [Route("api/listing/updatelisting")]
        public IHttpActionResult UpdateListing(Listing vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var listing = _listingRepository.GetById(vm.Id);
            if (listing == null) return BadRequest("Новина с такова id не можа да бъде намерена");
            listing.Body = vm.Body;
            listing.Heading = vm.Heading;
            listing.DateCreated = DateTime.Now;
            listing.CreatedBy = User.Identity.Name;
            _listingRepository.Update(listing);

            return Ok();
        }
    }
}
