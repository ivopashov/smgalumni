﻿using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using System.Web;

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
                Body = HttpContext.Current.Server.HtmlEncode(vm.Body),
                Heading = HttpContext.Current.Server.HtmlEncode(vm.Heading),
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now,
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
            var listing = _listingRepository.GetById(id);
            return Ok(listing);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/listing/skiptake")]
        public IHttpActionResult SkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var listings = _listingRepository
                .Page(skip, take)
                .Select(a => new { Heading = a.Heading, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName, Attachments = AutoMapper.Mapper.Map<List<AttachmentViewModel>>(a.Attachments) })
                .ToList();
            return Ok(listings);
        }

        [HttpGet]
        [Route("api/listing/my/skiptake")]
        public IHttpActionResult MyListingsSkipAndTake([FromUri] int take, [FromUri]int skip)
        {
            var listings = _listingRepository.PageListingForUser(CurrentUser.Id, skip, take)
                .Select(a => new { Heading = a.Heading, Body = a.Body, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName, Attachments = AutoMapper.Mapper.Map<List<AttachmentViewModel>>(a.Attachments) })
                .ToList();
            return Ok(listings);
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
            var count = _listingRepository.GetCount(CurrentUser.Id);
            return Ok(count);
        }

        //[HttpGet]
        //[Route("api/listing/mylistings")]
        //public IHttpActionResult MyListings()
        //{
        //    var listings = _listingRepository.ListingForUser(CurrentUser.Id)
        //        .Select(a => new { Heading = a.Heading, Body = a.Body, DateCreated = a.DateCreated, Id = a.Id, Enabled = a.Enabled, CreatedBy = a.User.UserName })
        //        .ToList();
        //    return Ok(listings);
        //}

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
        public IHttpActionResult UpdateListing(ListingUpdateVm vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            var listing = _listingRepository.GetById(vm.Id);
            if (listing == null)
            {
                return BadRequest("Обява с такова id не можа да бъде намерена");
            }
            if (listing.User.Id != CurrentUser.Id)
            {
                return Unauthorized();
            }
            listing.Body = HttpContext.Current.Server.HtmlEncode(vm.Body);
            listing.Heading = HttpContext.Current.Server.HtmlEncode(vm.Heading);
            listing.LastModified = DateTime.Now;
            var vmTempKeys = vm.Attachments.Select(a => a.TempKey);
            var listingTempKeys = listing.Attachments.Select(a => a.TempKey);

            var deletedAttachments = listingTempKeys.Where(a => !vmTempKeys.Contains(a.GetValueOrDefault())).ToList();
            foreach (var item in deletedAttachments)
            {
                var attachment = _attachmentRepository.FindByTempKey(item.GetValueOrDefault());
                listing.Attachments.Remove(attachment);
            }

            var addedAttachments = vmTempKeys.Where(a => !listingTempKeys.Contains(a)).ToList();
            foreach (var item in addedAttachments)
            {
                var attachment = _attachmentRepository.FindByTempKey(item);
                listing.Attachments.Add(attachment);
            }

            _listingRepository.Update(listing);

            return Ok();
        }
    }
}
