using Megaphone.Core.ClusterProviders;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Megaphone.Core
{
    public static class Cluster
    {
        private static IClusterProvider _clusterProvider;
        private static IFrameworkProvider _frameworkProvider;
        private static Uri _uri;
        private static ServiceData _serviceData;

        public static void BootstrapProviders(IFrameworkProvider frameworkProvider, IClusterProvider clusterProvider)
        {
            _clusterProvider = clusterProvider;
            _frameworkProvider = frameworkProvider;
        }

        public static async Task<ServiceInformation[]> FindServiceInstancesAsync(string name)
        {
            return await _clusterProvider.FindServiceInstancesAsync(name);
        }

        public static async Task<ServiceInformation> FindServiceInstanceAsync(string name)
        {
            return await _clusterProvider.FindServiceInstanceAsync(name);
        }

        public static async Task<Services[]> FindServiceByTagAsync(string[] tags)
        {
            return await _clusterProvider.FindServiceByTagAsync(tags);
        }

        public static void BootstrapClient(IClusterProvider clusterProvider)
        {
            _clusterProvider = clusterProvider;
            _clusterProvider.BootstrapClientAsync().Wait();
        }

        public static async Task KvPutAsync(string key, object value)
        {
            await _clusterProvider.KvPutAsync(key, value);
        }

        public static async Task<T> KvGetAsync<T>(string key)
        {
            return await _clusterProvider.KvGetAsync<T>(key);
        }

        public static Uri Bootstrap(IFrameworkProvider frameworkProvider, IClusterProvider clusterProvider, string serviceName, string version, string host = null, int? port = null, string[] tags = null, bool useHttps = false, bool selfRegisterTimer = false, int selfRegisterTimerInterval = 10000)
        {
            _serviceData = new ServiceData();

            if (selfRegisterTimer)
            {
                if (selfRegisterTimerInterval < 1000) selfRegisterTimerInterval = 1000;

                var timer = new Timer(selfRegisterTimerInterval);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }

            try
            {
                _frameworkProvider = frameworkProvider;

                if (host == null && port == null)
                {
                    _uri = _frameworkProvider.GetUri(serviceName, version, useHttps);
                }
                else
                {
                    _uri = useHttps ? new Uri($"https://{host}:{port}") : new Uri($"http://{host}:{port}");
                }

                var serviceId = serviceName + Guid.NewGuid();
                _clusterProvider = clusterProvider;

                if (tags != null)
                    _clusterProvider.RegisterServiceAsync(serviceName, serviceId, version, _uri, tags).Wait();
                else
                    _clusterProvider.RegisterServiceAsync(serviceName, serviceId, version, _uri).Wait();

                _serviceData.serviceName = serviceName;
                _serviceData.serviceId = serviceId;
                _serviceData.version = version;
                _serviceData._uri = _uri;
                _serviceData.tags = tags;

                return _uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _uri;
            }
        }

        private static async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ServiceInformation service = null;

                var services = await _clusterProvider.FindServiceInstancesAsync(_serviceData.serviceName);

                var serviceList = services.Where(x => x.Port == _serviceData._uri.Port).ToList();

                if (serviceList.Count == 1)
                {
                    service = services.SingleOrDefault(x => x.Port == _serviceData._uri.Port);
                }
                else
                {
                    for (int i = 0; i < serviceList.Count; i++)
                    {
                        if (i > 0)
                        {
                            var serviceId = serviceList[i].Id;
                            await _clusterProvider.DeRegisterServiceAsync(serviceId);
                        }
                    }
                }

                if (service == null && services.Length == 0)
                {
                    if (_serviceData.tags != null)
                        await _clusterProvider.RegisterServiceAsync(_serviceData.serviceName, _serviceData.serviceId, _serviceData.version, _serviceData._uri, _serviceData.tags);
                    else
                        await _clusterProvider.RegisterServiceAsync(_serviceData.serviceName, _serviceData.serviceId, _serviceData.version, _serviceData._uri);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}