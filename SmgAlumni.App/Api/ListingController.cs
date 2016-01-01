using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;

namespace SmgAlumni.App.Api
{
    public class ListingController : BaseApiController
    {
        private readonly IListingRepository _listingRepository;
        private readonly IAttachmentRepository _attachmentRepository;

        public ListingController(IAttachmentRepository attachmentRepository, IListingRepository listingRepository, IUserRepository userRepository, ILogger logger)
            : base(logger, userRepository)
        {
            _listingRepository = listingRepository;
            _attachmentRepository = attachmentRepository;
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
                Enabled = true,
                Attachments = GetAttachments(vm.TempKeys)
            };

            try
            {
                CurrentUser.Listings.Add(listing);
                _userRepository.Update(CurrentUser);
                DomainEvents.Raise(new AddListingDomainEvent() { Heading = vm.Heading, User = CurrentUser, Body = vm.Body });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Обявата не можа да бъде създадена");
            }
        }

        private List<Attachment> GetAttachments(List<Guid> tempKeys)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (var item in tempKeys)
            {
                var result = _attachmentRepository.FindByTempKey(item);
                if (result != null)
                {
                    attachments.Add(result);
                }
            }

            return attachments;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/listing/listingbyid")]
        public IHttpActionResult GetListingById([FromUri] int id)
        {
            var news = _listingRepository.GetById(id);
            return Ok(news);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/listing/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _listingRepository
                .Page(skip, take)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName })
                .ToList();
            return Ok(news);
        }

        [HttpGet]
        [Route("api/listing/my/skiptake")]
        public IHttpActionResult MyListingsSkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var news = _listingRepository.ListingForUser(CurrentUser.Id)
                .OrderBy(a => a.DateCreated).Take(take).Skip(skip)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName })
                .ToList();
            return Ok(news);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/listing/count")]
        public IHttpActionResult GetListingsCount()
        {
            var count = _listingRepository.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("api/listing/my/count")]
        public IHttpActionResult GetMyListingsCount()
        {
            var count = _listingRepository.ListingForUser(CurrentUser.Id).Count();
            return Ok(count);
        }

        [HttpGet]
        [Route("api/listing/mylistings")]
        public IHttpActionResult MyListings()
        {
            var listings = _listingRepository.ListingForUser(CurrentUser.Id)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName })
                .ToList();
            return Ok(listings);
        }

        [HttpGet]
        [Route("api/listing/delete")]
        public IHttpActionResult DeleteListing(int id)
        {
            var listings = _listingRepository.GetById(id);
            if (listings == null) return BadRequest("Обявата не беше намерена. Моля опитайте отново.");
            //if user is not the creator, not admin or super admin - refuse
            if (CurrentUser.Id != listings.User.Id && (!CurrentUser.Roles.Any(a => a.Name.Equals("Admin")) || !CurrentUser.Roles.Any(a => a.Equals("MasterAdmin"))))
                return BadRequest("Нямате права да изтриете тази обява. Тя е създадена от друг.");
            try
            {
                _listingRepository.Delete(listings);
                DomainEvents.Raise(new DeleteListingDomainEvent() { Heading = listings.Heading, User = CurrentUser });
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
            if (listing.User.Id != CurrentUser.Id) return Unauthorized();
            listing.Body = vm.Body;
            listing.Heading = vm.Heading;
            listing.DateCreated = DateTime.Now;
            _listingRepository.Update(listing);

            return Ok();
        }
    }
}
