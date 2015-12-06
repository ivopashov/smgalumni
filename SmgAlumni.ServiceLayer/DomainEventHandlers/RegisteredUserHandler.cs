using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class RegisteredUserHandler : IHandleDomainEvent<RegisterUserDomainEvent>
    {
        private readonly INewsLetterCandidateRepository _newsLetterCandidateRepository;

        public RegisteredUserHandler(INewsLetterCandidateRepository newsLetterCandidateRepository)
        {
            _newsLetterCandidateRepository = newsLetterCandidateRepository;
        }

        public void Handle(RegisterUserDomainEvent args)
        {
            _newsLetterCandidateRepository.Add(new EF.Models.NewsLetterCandidate()
            {
                CreatedBy = args.User,
                CreatedOn = DateTime.Now,
                Sent = false,
                HtmlBody = GetHtml(args),
                Type = NewsLetterItemType.UserRegistered,
            });
        }

        private string GetHtml(RegisterUserDomainEvent args)
        {
            var html = "<p>" + args.User.FirstName + " " + args.User.LastName + " от " + args.User.Division + " клас " + args.User.YearOfGraduation + " випуск се регистрира на " + string.Format("{0}-{1}-{2}", args.RegisteredOn.Day, args.RegisteredOn.Month, args.RegisteredOn.Year) + "</p>";
            return ParseImageLinks(html);
        }

        private string ParseImageLinks(string html)
        {
            //TODO
            return html;
        }
    }
}
