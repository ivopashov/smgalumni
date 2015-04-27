﻿using System;
using SimpleInjector;
using SimpleInjector.Extensions;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
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
            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            //Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //Container.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
            RegisterTypes(Container);
            DomainEvents.SetContainer(Container);
        }

        private static void RegisterTypes(Container container)
        {
            container.RegisterManyForOpenGeneric(typeof(IHandleDomainEvent<>),
                    container.RegisterAll,
                    AppDomain.CurrentDomain.GetAssemblies());
            container.Register<ActivityRepository, ActivityRepository>();
            container.Register<SmgAlumniContext, SmgAlumniContext>();
            container.Register<UserRepository, UserRepository>();
            container.Register<EFUserManager, EFUserManager>();
            container.Register<NotificationEnqueuer, NotificationEnqueuer>();
            container.Register<NotificationRepository, NotificationRepository>();
            container.Register<SettingRepository, SettingRepository>();
            container.Register<AppSettings, AppSettings>();
            container.Register<IAppSettingsRetriever, EFSettingsRetriever>();
        }
    }
}