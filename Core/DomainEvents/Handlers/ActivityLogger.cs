using NLog;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents.Handlers
{
    public class ActivityLogger :
        IHandleDomainEvent<AddNewsDomainEvent>,
        IHandleDomainEvent<DeleteNewsDomainEvent>,
        IHandleDomainEvent<ModifyNewsDomainEvent>,
        IHandleDomainEvent<AddCauseDomainEvent>,
        IHandleDomainEvent<ModifyCauseDomainEvent>,
        IHandleDomainEvent<DeleteCauseDomainEvent>,
        IHandleDomainEvent<DeleteListingDomainEvent>,
        IHandleDomainEvent<VerifyUserEvent>
    {
        private ActivityRepository _activityRepositorty;
        private UserRepository _userRepository;

        public ActivityLogger(ActivityRepository activityRepository, UserRepository userRepository)
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

        public void Handle(VerifyUserEvent args)
        {
            var activity = new Activity()
            {
                ActivityType = ActivityType.VerifyUser,
                Date = DateTime.Now,
                Description = "User with username: " + args.UserName + " was verified",
            };

            args.User.Activities.Add(activity);
            _userRepository.Update(args.User);

        }
    }
}
