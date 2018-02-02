using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LoanCare_Mobile_API.Filters;
using System.Net.Http.Headers;

namespace LoanCare_Mobile_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //GlobalConfiguration.Configuration.Filters.Add(new ApiAuthenticationFilter());
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
