﻿using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class AddedCauseHandler : IHandleDomainEvent<AddCauseDomainEvent>
    {
        private readonly INewsLetterCandidateRepository _newsLetterCandidateRepository;

        public AddedCauseHandler(INewsLetterCandidateRepository newsLetterCandidateRepository)
        {
            _newsLetterCandidateRepository = newsLetterCandidateRepository;
        }

        public void Handle(AddCauseDomainEvent args)
        {
            _newsLetterCandidateRepository.Add(new EF.Models.NewsLetterCandidate()
            {
                CreatedBy = args.User,
                CreatedOn = DateTime.Now,
                Sent = false,
                HtmlBody = GetHtml(args),
                Type = NewsLetterItemType.CauseAdded,
            });
        }

        private string GetHtml(AddCauseDomainEvent args)
        {
            return "<h4>" + args.Heading + "</h4>" + args.Body;
        }
    }
}
