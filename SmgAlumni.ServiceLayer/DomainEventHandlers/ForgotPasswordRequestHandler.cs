using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils.DomainEvents.Models;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.EfEmailQuerer.Templates;
using System;
using System.Linq;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class ForgotPasswordRequestHandler : IHandleDomainEvent<ForgotPasswordEvent>
    {
        private readonly INotificationEnqueuer _sender;
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;

        public ForgotPasswordRequestHandler(INotificationEnqueuer sender, UserRepository userRepository, IAccountService accountService)
        {
            _sender = sender;
            _accountService = accountService;
            _userRepository = userRepository;
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
                _sender.EnqueueNotification(new EmailNotificationOptions
                {
                    To = email,
                    Template = new ResetPasswordTemplate
                    {
                        UserName = username,
                        Link = link
                    }
                }, NotificationKind.ForgotPassword);
            }
        }
    }
}
