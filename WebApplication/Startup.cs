// Class that gets the web api started. It enables swagger, starts the authentication, and registers a couple of elements essential for the results to show.
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Http;
using LightInject;
using Swashbuckle.Application;
using WebApplication.Filters;
using WindowsService.Scripts;

[assembly: OwinStartup(typeof(WebApplication.Startup))]

namespace WebApplication
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            
            var apiConfig = new HttpConfiguration();

            apiConfig.Formatters.Remove(apiConfig.Formatters.XmlFormatter);

            apiConfig.MapHttpAttributeRoutes();

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