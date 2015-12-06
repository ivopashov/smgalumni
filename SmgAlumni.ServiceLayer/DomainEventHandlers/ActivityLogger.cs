using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Linq;

namespace SmgAlumni.ServiceLayer.DomainEventHandlers
{
    public class ActivityLogger :
        IHandleDomainEvent<AddNewsDomainEvent>,
        IHandleDomainEvent<DeleteNewsDomainEvent>,
        IHandleDomainEvent<ModifyNewsDomainEvent>,
        IHandleDomainEvent<AddCauseDomainEvent>,
        IHandleDomainEvent<ModifyCauseDomainEvent>,
        IHandleDomainEvent<DeleteCauseDomainEvent>,
        IHandleDomainEvent<DeleteListingDomainEvent>,
        IHandleDomainEvent<RegisterUserDomainEvent>,
        IHandleDomainEvent<ForgotPasswordEvent>
    {
        private IActivityRepository _activityRepositorty;
        private IUserRepository _userRepository;

        public ActivityLogger(IActivityRepository activityRepository, IUserRepository userRepository)
        {
            _activityRepositorty = activityRepository;
            _userRepository = userRepository;
        }

        public void Handle(AddNewsDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.AddNews,
                Date = DateTime.Now,
                Description = "News with heading " + args.Heading + " was created.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);
        }

        public void Handle(RegisterUserDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.RegisteredUser,
                Date = DateTime.Now,
                Description = "User with username " + args.User.UserName + " was registered.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);
        }

        public void Handle(ForgotPasswordEvent args)
        {
            var user = _userRepository.UsersByEmail(args.Email).SingleOrDefault();

            if (user != null)
            {
                var activity = new Activity()
                {
                    ActivityType = ActivityType.ForgotPassword,
                    Date = DateTime.Now,
                    Description = "User with username " + user.UserName + " issued a forgot password request.",
                };

                user.Activities.Add(activity);
                _userRepository.Update(user);
            }
        }

        public void Handle(DeleteNewsDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.DeleteNews,
                Date = DateTime.Now,
                Description = "News with heading " + args.Heading + " was deleted.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }

        public void Handle(ModifyNewsDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.ModifyNews,
                Date = DateTime.Now,
                Description = "News with heading " + args.Heading + " was modified.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);
        }

        public void Handle(AddCauseDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.AddCause,
                Date = DateTime.Now,
                Description = "Cause with heading " + args.Heading + " was added.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }

        public void Handle(DeleteCauseDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.DeleteCause,
                Date = DateTime.Now,
                Description = "Cause with heading " + args.Heading + " was deleted.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }

        public void Handle(DeleteListingDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.DeleteListing,
                Date = DateTime.Now,
                Description = "Listing with heading " + args.Heading + " was deleted.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }



        public void Handle(ModifyCauseDomainEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.ModifyCause,
                Date = DateTime.Now,
                Description = "Cause with heading " + args.Heading + " was modified.",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }
    }
}
