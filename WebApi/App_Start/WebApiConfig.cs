using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
//using WebApiContrib.Formatting.Jsonp;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //name: "ApiByAction",
            //routeTemplate: "api/{controller}/{action}/{id}",
            //defaults: new { id = RouteParameter.Optional }
            //);

            ////> install-package WebApiContrib.Formatting.JsonP
            //// to allow cross domain ajax calls
            //// and also change data type to 'jsonp' on ajax call

            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter, "callback");
            //config.Formatters.Insert(0, jsonpFormatter);

            //GlobalConfiguration.Configuration.AddJsonpFormatter(config.Formatters.JsonFormatter, "callback");

            ////CORS :: to allow cross domain ajax calls
            ////origins: "http://www.example.com", headers: "accept,content-type,origin,x-my-header", methods: "get,post"

            //// Folowing code allow cross domain ajax calls only to the mentioned urls, headers and methods. 
            //// EnableCorsAttribute cors = new EnableCorsAttribute("http://localhost:53065, http://localhost:1234", "accept,content-type,origin,x-my-header", "get,post");

            //// Folowing code allow cross domain ajax calls to all the urls, headers and methods. 
            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");

            //config.EnableCors(cors);

            ////CORS:: To Enable CORS controller wise or action method wise
            config.EnableCors();
        }
    }
}
