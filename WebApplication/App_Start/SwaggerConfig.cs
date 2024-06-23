﻿using System.Web.Http;
using WebActivatorEx;
using WebApplication;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApplication
{
    /// <summary>
    /// Configures Swagger for the WebApplication API documentation.
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Registers Swagger configuration settings.
        /// </summary>
        /// <remarks>
        /// This method sets up Swagger to document the WebApplicationAPI with a single version (v1).
        /// It includes XML comments from a specified path, uses full type names in schema IDs, describes enums as strings,
        /// and customizes the Swagger UI.
        /// </remarks>
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

        /// <summary>
        /// Gets the path to the XML comments file.
        /// </summary>
        /// <returns>The file path of the XML comments that should be included in the Swagger documentation.</returns>
        /// <remarks>
        /// This method constructs the path to the XML documentation file generated by the compiler.
        /// This file is used by Swagger to include summary comments in the API documentation.
        /// </remarks>
        private static string GetXmlCommentsPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + @"\bin\WebApplication.xml";
        }
    }
}