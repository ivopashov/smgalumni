using System;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.EfEmailQuerer.Templates;
using SmgAlumni.Utils.Membership;

namespace SmgAlumni.Utils.DomainEvents.Handlers
{
    public class VerifiedUserHandler : IHandleDomainEvent<VerifyUserEvent>
    {

        private readonly INotificationEnqueuer _sender;
        private readonly EFUserManager _userManager;

        public VerifiedUserHandler(INotificationEnqueuer sender, EFUserManager usermanager)
        {
            _sender = sender;
            _userManager = usermanager;
        }

        public void Handle(VerifyUserEvent args)
        {
            CreateAndSaveNotification(args);
        }

        public void CreateAndSaveNotification(VerifyUserEvent args)
        {
            var user = _userManager.GetUserByUserName(args.UserName);
            if (user == null) throw new Exception();

            _sender.EnqueueNotification(new EmailNotificationOptions
            {
                To = user.Email,
                Template = new VerifyUserTemplate
                {
                    UserName = user.UserName,
                    Link = "http://smg-alumni.com"
                }
            }, NotificationKind.UserVerified);
        }
    }
}
