using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.Membership;
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
        private readonly EFUserManager _userManager;
        private User _user;

        public ListingController(ListingRepository listingRepository, Logger logger, EFUserManager userManager)
            : base(logger)
        {
            _listingRepository = listingRepository;
            VerifyNotNull(_listingRepository);
            _userManager = userManager;
            VerifyNotNull(_userManager);
            _user = _userManager.GetUserByUserName(User.Identity.Name);
        }

        [HttpPost]
        [Route("api/listing/createlisting")]
        public IHttpActionResult CreateListing(CauseNewsViewModelWithoutId vm)
        {
            var user = _userManager.GetUserByUserName(User.Identity.Name);

            var listing = new Listing()
            {
                Body = vm.Body,
                Heading = vm.Heading,
                DateCreated = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Enabled = true,
                UserId = User == null ? 0 : user.Id
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
            var news = _listingRepository.GetAll()
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _listingRepository.GetAll().OrderBy(a => a.DateCreated).Take(take).Skip(skip).Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.CreatedBy }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/my/skiptake")]
        public IHttpActionResult MyListingsSkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _listingRepository.GetAll().Where(a => a.CreatedBy.ToLower().Equals(User.Identity.Name.ToLower()))
                .OrderBy(a => a.DateCreated).Take(take).Skip(skip)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.CreatedBy }).ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/count")]
        public IHttpActionResult GetListingsCount()
        {
            var count = _listingRepository.GetAll()
                .Where(a => a.CreatedBy.ToLower().Equals(User.Identity.Name.ToLower())).ToList().Count;
            return Ok(count);
        }

        [HttpGet]
        [Route("api/listing/my/count")]
        public IHttpActionResult GetMyListingsCount()
        {
            var count = _listingRepository.GetAll().ToList().Count;
            return Ok(count);
        }

        [HttpGet]
        [Route("api/listing/mylistings")]
        public IHttpActionResult MyListings()
        {
            var listings = _listingRepository.GetAll()
                .Where(a => a.CreatedBy.ToLower().Equals(User.Identity.Name.ToLower()))
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.CreatedBy })
                .ToList();
            return Ok(listings);
        }

        [HttpGet]
        [Route("api/listing/delete")]
        public IHttpActionResult DeleteListing(int id)
        {
            var user = _userManager.GetUserByUserName(User.Identity.Name);
            var listings = _listingRepository.GetById(id);
            if (listings == null) return BadRequest("Обявата не беше намерена. Моля опитайте отново.");
            //if user is not the creator, not admin or super admin - refuse
            if (user.Id != listings.UserId && (!user.Roles.Any(a => a.Equals("Admin")) || !user.Roles.Any(a => a.Equals("MasterAdmin")))) 
                return BadRequest("Нямате права да изтриете тази обява. Тя е създадена от друг.");
            try
            {
                _listingRepository.Delete(listings);
                DomainEvents.Raise<DeleteListingDomainEvent>(new DeleteListingDomainEvent() {Heading=listings.Heading,UserId=_user==null?0:_user.Id });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Обявата не можа да бъде изтрита");
            }
            
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
