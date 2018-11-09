using Microsoft.Extensions.Configuration;
using System.IO;

namespace Megaphone.Core.Util
{
    public static class GlobalVariables
    {
        private static IConfiguration Configuration { get; set; }

        public static string GetConfigurationValue(string value)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            var cs = Configuration[$"ConsulConfig:{value}"];

            return cs;
        }

        public static string Host { get; } = GetConfigurationValue("Host");
        public static string Port { get; } = GetConfigurationValue("Port");
        public static string StatusEndPoint { get; } = GetConfigurationValue("StatusEndPoint");
        public static string StatusEndPointFrequencyCheck { get; } = GetConfigurationValue("StatusEndPointFrequencyCheck");
    }
}