using System;
using System.Net;
using System.Net.Sockets;
using Mono.Zeroconf;
using Nancy.Hosting.Self;

namespace BernalService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string localIp = LocalIpAddress();
                var nancyHost = new NancyHost(new Uri("http://" + localIp + ":990"));
                nancyHost.Start();

                Console.WriteLine("Server started!");

                var service = new RegisterService();
                service.Name = "BernalService";
                service.RegType = "_ws._tcp";
                service.ReplyDomain = "local.";
                service.Port = 990;
                service.Register();

                Console.WriteLine("Service registered!");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
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
