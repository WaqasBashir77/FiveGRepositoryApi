using FiveGApi.Custom;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FiveGApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");//origins,headers,methods   
            config.EnableCors(cors);
            // Web API routes
            config.MapHttpAttributeRoutes();
            //Custom Exception Filter
            config.Filters.Add(new CustomExceptionFilter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "ApiByName",
             routeTemplate: "api/{controller}/{action}",
             defaults: null,
             constraints: new { name = @"^[a-z]+$" }
         );
            config.Routes.MapHttpRoute(
      name: "ApiById",
      routeTemplate: "api/{controller}/{action}/{id}",
      defaults: null,
      constraints: new { name = @"^[0-9]+$" }
  );
        }
    }
}
