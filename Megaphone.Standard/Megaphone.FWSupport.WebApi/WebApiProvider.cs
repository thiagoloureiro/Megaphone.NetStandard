using Megaphone.Core;
using System;

public class WebApiProvider : IFrameworkProvider
{
    public Uri GetUri(string serviceName, string version)
    {
        return Configuration.GetUri();
    }
}