using System;
using Nancy.Hosting.Self;

namespace BernalService
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(new Uri("http://localhost:990"));
            nancyHost.Start();

            Console.ReadKey();
        }
    }
}
