using System;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.EfEmailQuerer.Templates;
using SmgAlumni.Utils.Membership;

namespace SmgAlumni.Utils.DomainEvents.Handlers
{
    class ForgotPasswordRequestHandler : IHandleDomainEvent<ForgotPasswordEvent>
    {
        private readonly NotificationEnqueuer _sender;
        private readonly UserRepository _userRepository;
        private readonly EFUserManager _userManager;

        public ForgotPasswordRequestHandler(NotificationEnqueuer sender, UserRepository userRepository, EFUserManager usermanager)
        {
            _sender = sender;
            _userManager = usermanager;
            _userRepository = userRepository;
        }

        public void Handle(ForgotPasswordEvent args)
        {
            GenerateResetPassRequest(args.Email, args.RequestScheme, args.RequestAuthority);
        }

        private void GenerateResetPassRequest(string email, string requestScheme, string requestAuthority)
        {
            var guid = Guid.NewGuid();
            var user = _userManager.GetUserByEmail(email);
            if (user == null) throw new Exception();

            if (_userManager.AddResetPassRequest(user, guid))
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
