using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;

namespace DashBoard
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Код, выполняемый при запуске приложения
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            nd.MapNetworkDrive(WebConfigurationManager.AppSettings["images_ip"], "", WebConfigurationManager.AppSettings["images_username"], WebConfigurationManager.AppSettings["images_password"]);
        }
    }
}