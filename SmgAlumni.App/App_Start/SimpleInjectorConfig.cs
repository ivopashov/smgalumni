﻿using System;
using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.WebApi;
using SmgAlumni.App.Api;
using SmgAlumni.App.Logging;
using SmgAlumni.App.Shared;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.Identity;
using SmgAlumni.Utils.Membership;
using SmgAlumni.Utils.Settings;

namespace SmgAlumni.App.App_Start
{
    public static class SimpleInjectorConfig
    {
        public static Container Container { get; private set; }
        public static void Initialize()
        {
            // Create the container as usual.
            Container = new Container();

            // This is an extension method from the integration package.
            Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            Container.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
            RegisterTypes(Container);
            Container.Verify();
            DomainEvents.SetContainer(Container);
        }

        private static void RegisterTypes(Container container)
        {
            container.RegisterManyForOpenGeneric(typeof(IHandleDomainEvent<>),
                    container.RegisterAll,
                    AppDomain.CurrentDomain.GetAssemblies());

            var webLifestyle = new WebApiRequestLifestyle();
            container.Register<SmgAlumniContext, SmgAlumniContext>(webLifestyle);

            //Repos
            container.Register<IActivityRepository, ActivityRepository>();
            container.Register<ISettingRepository, SettingRepository>(); //fix
            container.Register<IUserRepository, UserRepository>();
            container.Register<INotificationRepository, NotificationRepository>();
            container.Register<ICauseRepository, CauseRepository>();
            container.Register<IForumAnswerRepository, ForumAnswerRepository>();
            container.Register<IForumCommentsRepository, ForumCommentsRepository>();
            container.Register<IForumThreadRepository, ForumThreadRepository>();
            container.Register<IListingRepository, ListingRepository>();
            container.Register<INewsRepository, NewsRepository>();
            container.Register<IRoleRepository, RoleRepository>(); //fix

            container.Register<EFUserManager, EFUserManager>();
            container.Register<UserManager, UserManager>();
            container.Register<INotificationEnqueuer, NotificationEnqueuer>();
            
            
            container.Register<AppSettings, AppSettings>();
            container.Register<IAppSettingsRetriever, EFSettingsRetriever>();
            
            container.RegisterWithContext<ILogger>(dependencyContext =>
            {
                return new NLogLogger();
            });
            container.RegisterInitializer<BaseApiController>(ctrl =>
            {
                ctrl.Users =
                    container.GetInstance<UserRepository>();
            });
        }
    }
}