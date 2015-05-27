using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace SmgAlumni.App.ExceptionHandlers
{
    public class WebApiGlobalExceptionHandler : ExceptionLogger
    {
        private readonly Logger _logger;


        public WebApiGlobalExceptionHandler()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Log(LogLevel.Error, RequestToString(context.Request), context.Exception);
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}