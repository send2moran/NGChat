using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace NGChat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static RouteBase signalRHubRoute;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes, ref signalRHubRoute);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // https://gist.github.com/davidfowl/4692934 - Using readonly sessions with SignalR
            if (IsSignalRRequest(Context))
            {
                // Turn readonly sessions on for SignalR
                Context.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            }
        }

        private bool IsSignalRRequest(HttpContext context)
        {
            RouteData routeData = signalRHubRoute.GetRouteData(new HttpContextWrapper(context));

            // If the routeData isn't null then it's a SignalR request
            return routeData != null;
        }
    }
}