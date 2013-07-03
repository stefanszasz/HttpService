using System;
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
                var nancyHost = new NancyHost(new Uri("http://192.168.231.73:990"));
                nancyHost.Start();

                Console.WriteLine("Server started!");

                try
                {
                    RegisterService service = new RegisterService();
                    service.Name = "BernalService";
                    service.RegType = "_http._tcp";
                    service.ReplyDomain = "local.";
                    service.Port = 990;
                    service.Register();
                }
                catch (Exception ex)
                {
                    int i = 0;
                }

                Console.WriteLine("Service registered!");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }
    }
}
