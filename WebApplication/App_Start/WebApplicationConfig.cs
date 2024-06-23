// Redirects directly to Swagger page
using Swashbuckle.Application;
using System.Web.Http;

namespace WebApplication
{
    /// <summary>
    /// Configures the HTTP services for the WebApplication.
    /// </summary>
    public static class WebApplicationConfig
    {
        /// <summary>
        /// Registers the configuration settings for the Web API.
        /// </summary>
        /// <param name="config">The HTTP configuration to be modified.</param>
        /// <remarks>
        /// This method clears existing routes, sets up attribute routing, and configures a default route to redirect to the Swagger UI.
        /// </remarks>
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
