using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebApiThrottle;
using SmgAlumni.App.ExceptionHandlers;
using SmgAlumni.Utils;
using SmgAlumni.App.Filters;

namespace SmgAlumni.App.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            //remove xml
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            config.Services.Add(typeof(IExceptionLogger), new WebApiGlobalExceptionHandler());

            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = new ThrottlePolicy(perSecond: 2, perMinute: 30, perHour: 200, perDay: 1500, perWeek: 3000)
                {
                    IpThrottling = true,
                   EndpointThrottling=true 
                },
                Repository = new CacheRepository()
            });

            config.Filters.Add(new UnhandledApiExceptionAttribute(SimpleInjectorConfig.Container.GetInstance<ILogger>()));
        }

        public static void UseJsonFormatter()
        {
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}