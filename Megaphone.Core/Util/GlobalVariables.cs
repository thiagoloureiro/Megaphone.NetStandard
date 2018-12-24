using Microsoft.Extensions.Configuration;
using System.IO;

public static class GlobalVariables
{
    private static IConfiguration Configuration { get; set; }

    public static string Host { get; } = GetConfigurationValue("Host");

    public static string Port { get; } = GetConfigurationValue("Port");

    public static string StatusEndPoint { get; } = GetConfigurationValue("StatusEndPoint");

    public static string StatusEndPointFrequencyCheck { get; } = GetConfigurationValue("StatusEndPointFrequencyCheck");

    public static string GetConfigurationValue(string value)
    {
        Configuration = JsonConfigurationExtensions.AddJsonFile(FileConfigurationExtensions.SetBasePath(new ConfigurationBuilder(), Directory.GetCurrentDirectory()), "appsettings.json").Build();
        return Configuration.GetSection("ConsulConfig:" + value).Value;
    }
}