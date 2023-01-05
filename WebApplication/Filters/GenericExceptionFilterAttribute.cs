using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebApplication.Filters
{
    public class GenericExceptionFilterAttribute : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.MonitoringLogger.Error($"Exception: {context.Exception.ToString()}");
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message);
        }
    }
}