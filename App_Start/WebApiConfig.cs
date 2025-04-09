using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace EmployeeService
{
    //public class CustomJsonFormatter : JsonMediaTypeFormatter
    //{
    //    public CustomJsonFormatter()
    //    {
    //        this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
    //    }
    //    public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
    //    {
    //        base.SetDefaultContentHeaders(type, headers, mediaType); 
    //        headers.Add("Content-Type", "application/json");
    //    }
    //}
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            /*change Json output with Pascal casing(FirstName) to camel casing(firstName).Can do same for xml as well*/
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            /*return only in Json from Web API service irrespective of header value(ex.if we put Accept:application/xml in header,then also get only Json)*/
            //config.Formatters.Remove(config.Formatters.XmlFormatter); //removed xml ,so only json will return
            //config.Formatters.Remove(config.Formatters.JsonFormatter);//to get nly xml data
            
            /*Return Json instead of Xml from web api service when request is from browser(content-type should be json not text/html) but in fiddler or postman,show according to the set accept value 
             * For that create customformatter class and then Register custom formatter in Webconfig file*/
            //config.Formatters.Add(new CustomJsonFormatter());

        }
    }
}
