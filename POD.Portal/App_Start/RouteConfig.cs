using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace POD.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "HomeCatchAllRoute",
                url: "Home/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CustomersCatchAllRoute",
                url: "Customers/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );         

            routes.MapRoute(
                name: "AccountCatchAllRoute",
                url: "Account/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "ReportCatchAllRoute",
              url: "Report/{*.}",
              defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UserCatchAllRoute",
                url: "User/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TicketssCatchAllRoute",
                url: "Search/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ProductsCatchAllRoute",
                url: "Products/{*.}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );



            //TODO
            // routes.MapRoute(
            //    name: "Default",
            //    url: "{*url}",
            //    defaults: new { controller = "Home", action = "Index" }
            //);
        }
    }
}
