using System;

namespace Megaphone.Core
{
    public interface IFrameworkProvider
    {
        Uri GetUri(string serviceName, string version);
    }
}