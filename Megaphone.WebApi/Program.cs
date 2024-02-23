using Megaphone.Core;
using Megaphone.Core.ClusterProviders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Megaphone.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var uri = Cluster.Bootstrap(new WebApiProvider(), new ConsulProvider(), "values", "v1",
                tags: new[] { "provider", "providers", "providers2" }, host: "localhost", port: 5001, useHttps: false,
                selfRegisterTimer: true, selfRegisterTimerInterval: 10000);
            CreateWebHostBuilder(args, uri).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, Uri uri) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://127.0.0.1:{uri.Port}");
    }
}