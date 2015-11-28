using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.EfEmailQuerer.Templates;
using System;
using System.Linq;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class VerifiedUserHandler : IHandleDomainEvent<VerifyUserEvent>
    {
        private readonly INotificationEnqueuer _sender;
        private readonly IUserRepository _userRepository;

        public VerifiedUserHandler(INotificationEnqueuer sender, IUserRepository userRepository)
        {
            _sender = sender;
            _userRepository = userRepository;
        }

        public void Handle(VerifyUserEvent args)
        {
            CreateAndSaveNotification(args);
        }

        public void CreateAndSaveNotification(VerifyUserEvent args)
        {
            var user = _userRepository.UsersByUserName(args.UserName).SingleOrDefault();
            if (user == null)
            {
                throw new NullReferenceException("User with username: " + args.UserName + " cound not be retrieved");
            }

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
