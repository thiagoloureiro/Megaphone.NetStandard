using Megaphone.Core;
using Megaphone.Core.ClusterProviders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Megaphone.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Host and Port are optional, if not defined they will get automatically (dynamic)
            var uri = Cluster.Bootstrap(new WebApiProvider(), new ConsulProvider(), "values", "v1");
            CreateWebHostBuilder(args, uri).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, Uri uri) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://127.0.0.1:{uri.Port}");
    }
}