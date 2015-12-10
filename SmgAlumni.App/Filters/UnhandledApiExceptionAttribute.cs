using System.Web.Http.Filters;
using SmgAlumni.Utils;

namespace SmgAlumni.App.Filters
{
    public class UnhandledApiExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public UnhandledApiExceptionAttribute(ILogger logger)
        {
            this.logger = logger;
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                this.logger.Error("Unhandled exception in API " + context.Exception);
                if (context.Exception.InnerException != null)
                {
                    this.logger.Error("Unhandled exception in API " + context.Exception.InnerException);
                }

                //if (context.Exception is ValidationException)
                //{
                //    var validationException = (ValidationException)context.Exception;
                //    throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.BadRequest, validationException.Errors));
                //}
                //else if (!(context.Exception is HttpResponseException))
                //{
                //    var returnAsBadRequest = context.Exception is RepositoryException || context.Exception is NotSupportedException;
                //    var errorCode = returnAsBadRequest ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
                //    throw new HttpResponseException(context.Request.CreateResponse(errorCode, context.Exception.Message));
                //}
            }
        }
    }
}