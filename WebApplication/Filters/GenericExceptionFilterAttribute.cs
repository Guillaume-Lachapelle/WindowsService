using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebApplication.Filters
{
    /// <summary>
    /// Represents a filter attribute that handles exceptions thrown by action methods.
    /// </summary>
    public class GenericExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Called when an exception is thrown during the execution of the action method.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        /// <remarks>
        /// This method logs the exception using the <see cref="Logger.ExceptionLogger"/> and creates a new <see cref="HttpResponseMessage"/>
        /// with the <see cref="HttpStatusCode.InternalServerError"/> status code and the exception message.
        /// </remarks>
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.ExceptionLogger.Error($"Exception: {context.Exception.ToString()}");
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message);
        }
    }
}