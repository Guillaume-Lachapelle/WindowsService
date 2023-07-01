// Redirects directly to Swagger page
using Swashbuckle.Application;
using System.Web.Http;

namespace WebApplication
{
    public static class WebApplicationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Remove existing routes
            config.Routes.Clear();

            // Other configuration settings for Web API
            config.MapHttpAttributeRoutes();

            // Redirect to Swagger UI
            config.Routes.MapHttpRoute(
                name: "Swagger UI",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger/ui/index")
            );
        }
    }
}
