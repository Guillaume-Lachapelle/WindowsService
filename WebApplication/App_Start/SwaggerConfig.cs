using System.Web.Http;
using WebActivatorEx;
using WebApplication;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApplication
{
    public class SwaggerConfig
    {   
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WebApplicationAPI");
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.UseFullTypeNameInSchemaIds();
                c.DescribeAllEnumsAsStrings();
            }).EnableSwaggerUi(c => { 
                c.DocumentTitle("WebApplicationDocumentation");
                c.EnableDiscoveryUrlSelector();
            });
        }
        private static string GetXmlCommentsPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + @"\bin\WebApplication.xml";
        }
    }
}