using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.WebApi;
using SmgAlumni.App.Api;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.Membership;
using SmgAlumni.Utils.Settings;
using SmgAlumni.App.Logging;
using SmgAlumni.App.Shared;
using SmgAlumni.Utils.Identity;

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
            DomainEvents.SetContainer(Container);
        }

        private static void RegisterTypes(Container container)
        {
            container.RegisterManyForOpenGeneric(typeof(IHandleDomainEvent<>),
                    container.RegisterAll,
                    AppDomain.CurrentDomain.GetAssemblies());

            var webLifestyle = new WebApiRequestLifestyle();
            container.Register<SmgAlumniContext, SmgAlumniContext>(webLifestyle);
            container.Register<ActivityRepository, ActivityRepository>();
            container.Register<UserRepository, UserRepository>();
            container.Register<EFUserManager, EFUserManager>();
            container.Register<UserManager, UserManager>();
            container.Register<NotificationEnqueuer, NotificationEnqueuer>();
            container.Register<NotificationRepository, NotificationRepository>();
            container.Register<SettingRepository, SettingRepository>();
            container.Register<AppSettings, AppSettings>();
            container.Register<IAppSettingsRetriever, EFSettingsRetriever>();
            container.RegisterWithContext<ILogger>(dependencyContext =>
            {
                var name = "WebAPI";
                return new NLogLogger(name);
            });
            container.RegisterInitializer<BaseApiController>(ctrl =>
            {
                ctrl.Users =
                    container.GetInstance<UserRepository>();
            });
        }
    }
}