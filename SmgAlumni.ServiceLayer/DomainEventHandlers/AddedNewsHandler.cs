using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using System;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class AddedNewsHandler : IHandleDomainEvent<AddNewsDomainEvent>
    {
        private readonly INewsLetterCandidateRepository _newsLetterCandidateRepository;

        public AddedNewsHandler(INewsLetterCandidateRepository newsLetterCandidateRepository)
        {
            _newsLetterCandidateRepository = newsLetterCandidateRepository;
        }

        public void Handle(AddNewsDomainEvent args)
        {
            _newsLetterCandidateRepository.Add(new EF.Models.NewsLetterCandidate()
            {
                CreatedBy = args.User,
                CreatedOn = DateTime.Now,
                Sent = false,
                HtmlBody = GetHtml(args),
                Type = NewsLetterItemType.NewsAdded,
            });
        }

        private string GetHtml(AddNewsDomainEvent args)
        {
            return"<h4>" + args.Heading + "</h4>" + args.Body;
        }
    }
}
