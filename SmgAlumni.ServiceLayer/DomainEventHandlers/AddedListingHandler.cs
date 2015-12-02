using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using System;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class AddedListingHandler : IHandleDomainEvent<AddListingDomainEvent>
    {
        private readonly INewsLetterCandidateRepository _newsLetterCandidateRepository;

        public AddedListingHandler(INewsLetterCandidateRepository newsLetterCandidateRepository)
        {
            _newsLetterCandidateRepository = newsLetterCandidateRepository;
        }

        public void Handle(AddListingDomainEvent args)
        {
            _newsLetterCandidateRepository.Add(new EF.Models.NewsLetterCandidate()
            {
                CreatedBy = args.User,
                CreatedOn = DateTime.Now,
                Sent = false,
                HtmlBody = GetHtml(args),
                Type = NewsLetterItemType.ListingAdded
            });
        }

        private string GetHtml(AddListingDomainEvent args)
        {
            var html = "<h3>" + args.Heading + "</h3></br>" + args.Body;
            return ParseImageLinks(html);
        }

        private string ParseImageLinks(string html)
        {
            //TODO
            return html;
        }
    }
}
