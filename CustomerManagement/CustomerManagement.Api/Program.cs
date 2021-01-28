using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace CustomerManagement.Api
{
    internal class Program
    {
        private const int Port = 5000;

        private static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static int GetPort()
        {
            var port = Environment.GetEnvironmentVariable("Port");
            int.TryParse(port, out var portNumber);
            return portNumber == 0 ? Port : portNumber;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseKestrel(options => { options.ListenAnyIP(GetPort()); })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseSerilog();
        }
    }
}