using Megaphone.Core;
using System;

public class WebApiProvider : IFrameworkProvider
{
    public Uri GetUri(string serviceName, string version, bool useHttps)
    {
        return Configuration.GetUri(useHttps: useHttps);
    }
}