using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector.Extensions;
using System.Web.Http;
using SmgAlumni.Data.Repositories;
using SmgAlumni.Utils.Membership;
using SmgAlumni.Utils.EfEmailQuerer;
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
            container.Register<SmgAlumni.EF.DAL.SmgAlumniContext, SmgAlumni.EF.DAL.SmgAlumniContext>();
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