using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                           name: null,
                           url: "Page{currentPage}",
                           defaults: new { controller = "Home", action = "Index" },
                           constraints: new { currentPage = @"\d+" }
                           );

            routes.MapRoute(
                          name: null,
                          url: "News/{blogId}",
                          defaults: new { controller = "Home", action = "NewsDetails" },
                          constraints: new { blogId = @"\d+" }
                      );

            routes.MapRoute(
                        name: null,
                        url: "About",
                        defaults: new { controller = "Home", action = "About" }
                    );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
