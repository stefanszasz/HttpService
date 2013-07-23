using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Mono.Zeroconf.Providers.Bonjour;

namespace Service.Nancy
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            string localIp = LocalIpAddress();
          
            var service = new RegisterService();
            service.Name = "RemoteControllerServiceI";
            service.RegType = "_ws._tcp,_remotec,_sub";
            service.ReplyDomain = "local.";
            service.Port = 8000;
            service.Register();

            var service2 = new RegisterService();
            service2.Name = "RemoteControllerService";
            service2.RegType = "_ws._tcp,_remotec,_sub";
            service2.ReplyDomain = "local.";
            service2.Port = 8000;
            service2.Register();
        }

        public static string LocalIpAddress()
        {
            string localIp = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }
    }
}