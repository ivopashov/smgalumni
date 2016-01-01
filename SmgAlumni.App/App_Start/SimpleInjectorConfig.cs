using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.WebApi;
using SmgAlumni.App.Logging;
using SmgAlumni.App.Shared;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.ServiceLayer;
using SmgAlumni.ServiceLayer.DomainEventHandlers;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.Settings;
using System;
using System.Web.Http;

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
            container.Register<ISettingRepository, SettingRepository>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IAccountNotificationRepository, AccountNotificationRepository>();
            container.Register<ICauseRepository, CauseRepository>();
            container.Register<IForumAnswerRepository, ForumAnswerRepository>();
            container.Register<IForumCommentsRepository, ForumCommentsRepository>();
            container.Register<IForumThreadRepository, ForumThreadRepository>();
            container.Register<IListingRepository, ListingRepository>();
            container.Register<INewsRepository, NewsRepository>();
            container.Register<IRoleRepository, RoleRepository>();
            container.Register<IAttachmentRepository, AttachmentRepository>();
            container.Register<INewsLetterCandidateRepository, NewsLetterCandidateRepository>();

            container.Register<IUserService, UserService>();
            container.Register<IAccountService, AccountService>();
            container.Register<IAppSettings, AppSettings>();
            container.Register<IAppSettingsRetriever, EFSettingsRetriever>();
            container.Register<IRequestSender, RequestSender>();
            container.Register<INewsLetterGenerator, NewsLetterGenerator>();

            container.RegisterWithContext<ILogger>(dependencyContext =>
            {
                return new NLogLogger();
            });
        }
    }
}