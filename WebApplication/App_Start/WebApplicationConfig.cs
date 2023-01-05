// Redirects directly to Swagger page
using Swashbuckle.Application;
using System.Web.Http;

namespace WebApplication
{
    public static class WebApplicationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Get the web application to navigate to the swagger page
            config.Routes.MapHttpRoute(
                name: "Swagger UI",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index")
            );
        }
    }
}