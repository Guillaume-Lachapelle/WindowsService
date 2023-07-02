﻿using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Http;
using LightInject;
using Swashbuckle.Application;
using WebApplication.Filters;
using WindowsService.Helpers;

[assembly: OwinStartup(typeof(WebApplication.Startup))]

namespace WebApplication
{
    public class Startup
    {
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
