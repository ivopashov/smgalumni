using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Data.Repositories;
using SmgAlumni.Utils.Membership;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.EfEmailQuerer.Templates;

namespace SmgAlumni.Utils.DomainEvents.Handlers
{
    public class VerifiedUserHandler : IHandleDomainEvent<VerifyUserEvent>
    {

        private readonly NotificationEnqueuer _sender;
        private readonly UserRepository _userRepository;
        private readonly EFUserManager _userManager;

        public VerifiedUserHandler(NotificationEnqueuer sender, UserRepository userRepository, EFUserManager usermanager)
        {
            _sender = sender;
            _userManager = usermanager;
            _userRepository = userRepository;
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
                    Link = "dfsf"
                }
            }, EF.Models.enums.NotificationKind.UserVerified);
        }
    }
}
