using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace DashBoard
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute("Tasks","Tasks/TasksList", "~/Tasks.aspx");
            routes.MapPageRoute("Task", "Tasks/Task/{id}", "~/Tasks/_Task.aspx");
        }
    }
}
