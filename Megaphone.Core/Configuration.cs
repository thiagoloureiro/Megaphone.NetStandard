using System;
using System.Net;
using System.Net.Sockets;

namespace Megaphone.Core
{
    public static class Configuration
    {
        public static Uri GetUri(int port = 0, bool useHttps = false)
        {
            port = port == 0 ? FreeTcpPort() : port;
            Uri uri;

            uri = useHttps ? new Uri("https://localhost:" + port) : new Uri("http://localhost:" + port);

            return uri;
        }

        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}