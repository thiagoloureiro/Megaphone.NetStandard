using Megaphone.Core.ClusterProviders;
using System;
using System.Threading.Tasks;

namespace Megaphone.Core
{
    public static class Cluster
    {
        private static IClusterProvider _clusterProvider;
        private static IFrameworkProvider _frameworkProvider;
        private static Uri _uri;

        public static Task<ServiceInformation[]> FindServiceInstancesAsync(string name)
        {
            return _clusterProvider.FindServiceInstancesAsync(name);
        }

        public static Task<ServiceInformation> FindServiceInstanceAsync(string name)
        {
            return _clusterProvider.FindServiceInstanceAsync(name);
        }

        public static void BootstrapClient(IClusterProvider clusterProvider)
        {
            _clusterProvider = clusterProvider;
            _clusterProvider.BootstrapClientAsync().Wait();
        }

        public static Task KvPutAsync(string key, object value)
        {
            return _clusterProvider.KvPutAsync(key, value);
        }

        public static Task<T> KvGetAsync<T>(string key)
        {
            return _clusterProvider.KvGetAsync<T>(key);
        }

        public static Uri Bootstrap(IFrameworkProvider frameworkProvider, IClusterProvider clusterProvider, string serviceName, string version, string host = null, int? port = null)
        {
            try
            {
                _frameworkProvider = frameworkProvider;

                if (host == null && port == null)
                {
                    _uri = _frameworkProvider.GetUri(serviceName, version);
                }
                else
                {
                    _uri = new Uri($"http://{host}:{port}");
                }

                var serviceId = serviceName + Guid.NewGuid();
                _clusterProvider = clusterProvider;
                _clusterProvider.RegisterServiceAsync(serviceName, serviceId, version, _uri).Wait();
                return _uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _uri;
            }
        }
    }
}