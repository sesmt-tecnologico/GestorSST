using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GISWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            var defaultController = ConfigurationManager.AppSettings["Web:DefaultController"].ToString();
            var defaultAction = ConfigurationManager.AppSettings["Web:DefaultAction"].ToString();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = defaultController, action = defaultAction, id = UrlParameter.Optional }
            );
        }
    }
}
