using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JaCoolMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Products",
            url: "Home/Products",
            defaults: new { controller = "Home", action = "Products" }
            );

            routes.MapRoute(
            name: "AboutUs",
            url: "Home/AboutUs",
            defaults: new { controller = "Home", action = "AboutUs" }
            );

            routes.MapRoute(
            name: "Login",
            url: "Home/Login",
            defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
            name: "Admin",
            url: "Home/Admin",
            defaults: new { controller = "Home", action = "Admin" }
            );
        }
    }
}