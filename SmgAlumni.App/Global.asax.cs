using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SmgAlumni.App.App_Start;
using SmgAlumni.App.Filters;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils;
using SmgAlumni.Utils.Mapping;

namespace SmgAlumni.App
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.UseJsonFormatter();
            MapInitializer.Initialize();
            SimpleInjectorConfig.Initialize();
            Database.SetInitializer<SmgAlumniContext>(null);
        }

        public static void RegisterActionFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new UnhandledApiExceptionAttribute(SimpleInjectorConfig.Container.GetInstance<ILogger>()));
        }
    }
}
