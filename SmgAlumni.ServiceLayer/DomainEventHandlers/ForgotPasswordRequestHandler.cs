using RestSharp;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils.DomainEvents.Models;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.EfEmailQuerer.Templates;
using SmgAlumni.Utils.Settings;
using System;
using System.Linq;
using System.Net;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class ForgotPasswordRequestHandler : IHandleDomainEvent<ForgotPasswordEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly IRequestSender _requestSender;
        private readonly IAppSettings _appSettings;
        private int SendRetries = 0;

        public ForgotPasswordRequestHandler(UserRepository userRepository, IAccountService accountService, IRequestSender requestSender, IAppSettings appSettings)
        {
            _accountService = accountService;
            _userRepository = userRepository;
            _requestSender = requestSender;
            _appSettings = appSettings;
        }

        public void Handle(ForgotPasswordEvent args)
        {
            GenerateResetPassRequest(args.Email, args.RequestScheme, args.RequestAuthority);
        }

        private void GenerateResetPassRequest(string email, string requestScheme, string requestAuthority)
        {
            var guid = Guid.NewGuid();
            var user = _userRepository.UsersByEmail(email).SingleOrDefault();
            if (user == null)
            {
                throw new NullReferenceException("User with email: " + email + " could not be retrieved");
            }

            if (_accountService.AddResetPassRequest(user, guid))
            {
                string link = requestScheme + "://" + requestAuthority + "/#/resetpassword/" + guid + "/" + user.Email;
                CreateAndEnqueueMessage(link, user.Email, user.UserName);
            }
        }

        private void CreateAndEnqueueMessage(string link, string email, string username)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var template = new ResetPasswordTemplate()
                {
                    Link = link,
                    UserName = username
                };

                var result = SendRequest(template.Template, template.Subject, email);

                if (result == HttpStatusCode.OK || result == HttpStatusCode.Accepted || result == HttpStatusCode.Created)
                {
                    return;
                }
                else
                {
                    SendRetries++;
                    if (SendRetries < 3)
                    {
                        CreateAndEnqueueMessage(link, email, username);
                    }

                    return;
                }

            }
        }

        private HttpStatusCode SendRequest(string message, string subject, string email)
        {
            return _requestSender.InitializeClient()
               .AddParameter("domain", "www.smg-alumni.com", ParameterType.UrlSegment)
                        .SetResource("{domain}/messages")
                        .AddParameter("from", _appSettings.MailgunSettings.From)
                        .AddParameter("subject", subject)
                        .AddParameter("html", message)
                        .AddParameter("to", email)
                        .SetMethod(Method.POST)
                        .Execute()
                        .StatusCode;
        }
    }
}
