using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingCartMvc
{
    public class RouteConfig
    {
       
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           // routes.MapRoute(
           //     name:  "Default",
           //     url:  "{controller}/{action}/{id}",
           //     defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           //    //,namespaces: new[] { "ShoppingCartMvc.Controllers" }
           //).DataTokens.Add("Areas", "Admin");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ShoppingCartMvc.Controllers" }
            );
        }
    }
}
