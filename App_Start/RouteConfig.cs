using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebLightNovel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");         
            routes.MapRoute(
                name: "MemberInfo",
                url: "thanh-vien/{id}",
                defaults: new { controller = "Account", action = "Infor", id = UrlParameter.Optional },
                namespaces: new string[] { "WebLightNovel.Controllers" }
            );

            routes.MapRoute(
                name: "History",
                url: "lich-su-doc",
                defaults: new { controller = "Home", action = "History" },
                namespaces: new string[] { "WebLightNovel.Controllers" }
            );

            routes.MapRoute(
                name: "StoryDetail",
                url: "truyen/{id}/{story_name}",
                defaults: new { controller = "Story", action = "Detail", story_name = UrlParameter.Optional, id = UrlParameter.Optional },
                constraints : new { id = @"\d+" },
                namespaces: new string[] { "WebLightNovel.Controllers" }
            );

            routes.MapRoute(
                name: "StoryDetailChapter",
                url: "truyen/{id_story}/{story_name}/{id}/{chapter_name}",
                defaults: new { controller = "Story", action = "DetailChapter", story_name = UrlParameter.Optional, id = UrlParameter.Optional, id_stoy = UrlParameter.Optional, chapter_name = UrlParameter.Optional },
                namespaces: new string[] { "WebLightNovel.Controllers" }
            );

            routes.MapRoute(
                 name: "StoryCategory",
                 url: "the-loai/{category_name}",
                 defaults: new { controller = "Story", action = "Category", category_name = UrlParameter.Optional },
                 namespaces: new string[] { "WebLightNovel.Controllers" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
