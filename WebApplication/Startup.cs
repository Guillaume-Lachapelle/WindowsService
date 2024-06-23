using Microsoft.Owin;
using Owin;
using System.Web.Http;
using LightInject;
using Swashbuckle.Application;
using WebApplication.Filters;
using WindowsService.Helpers;

[assembly: OwinStartup(typeof(WebApplication.Startup))]

namespace WebApplication
{
    /// <summary>
    /// Configures the application's startup settings and services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application and HTTP services.
        /// </summary>
        /// <param name="app">The application builder used to configure the application.</param>
        /// <remarks>
        /// This method sets up the HTTP configuration, including routing, formatters, and exception filters.
        /// It also configures Swagger for API documentation and initializes dependency injection.
        /// </remarks>
        public void Configuration(IAppBuilder app)
        {
            var apiConfig = new HttpConfiguration();
            apiConfig.Formatters.Remove(apiConfig.Formatters.XmlFormatter);
            apiConfig.MapHttpAttributeRoutes();

            // Call the Register method from WebApplicationConfig
            WebApplicationConfig.Register(apiConfig);

            apiConfig.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WebApplication");
            }).EnableSwaggerUi();

            var container = new ServiceContainer();
            apiConfig.Filters.Add(new GenericExceptionFilterAttribute());
            container.RegisterApiControllers();
            container.EnableWebApi(apiConfig);
            app.UseWebApi(apiConfig);

            DatabaseHandler handler = new DatabaseHandler();
            Logger.MonitoringLogger.Info("Web app finished connecting");
        }
    }
}
