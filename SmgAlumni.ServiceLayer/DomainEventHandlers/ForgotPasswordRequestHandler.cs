using RestSharp;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
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
        private readonly IAppSettings _appSettings;
        private readonly IAccountNotificationRepository _notificationRepository;
        private int SendRetries = 0;

        public ForgotPasswordRequestHandler(IUserRepository userRepository, IAccountService accountService, IAppSettings appSettings, IAccountNotificationRepository notificationRepository)
        {
            _accountService = accountService;
            _userRepository = userRepository;
            _appSettings = appSettings;
            _notificationRepository = notificationRepository;
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
                return;
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

                var notification = new Notification()
                {
                    CreatedOn = DateTime.Now,
                    Kind = EF.Models.enums.NotificationKind.ForgotPassword,
                    Sent = false,
                    SentOn = null,
                    To = email,
                    HtmlMessage = template.Template,
                    Message = null
                };

                _notificationRepository.Add(notification);
            }
        }
    }
}
