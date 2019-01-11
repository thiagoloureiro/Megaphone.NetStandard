using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megaphone.Core.ClusterProviders
{
    public class ConsulProvider : IClusterProvider
    {
        private readonly int _consulPort;

        public ConsulProvider(int port = 0)
        {
            _consulPort = port;
        }

        public async Task<ServiceInformation[]> FindServiceInstancesAsync(string name)
        {
            var x = new ConsulRestClient();
            var res = await x.FindServiceAsync(name).ConfigureAwait(false);

            return res;
        }

        public async Task RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri)
        {
            var x = new ConsulRestClient();
            await x.RegisterServiceAsync(serviceName, serviceId, uri).ConfigureAwait(false);
            StartReaper();
        }

        public Task BootstrapClientAsync()
        {
            StartReaper();
            return Task.FromResult(0);
        }

        public async Task KvPutAsync(string key, object value)
        {
            var x = new ConsulRestClient();
            await x.KVPutAsync(key, value).ConfigureAwait(false);
        }

        public async Task<T> KvGetAsync<T>(string key)
        {
            var x = new ConsulRestClient();
            return await x.KVGetAsync<T>(key).ConfigureAwait(false); ;
        }

        private void StartReaper()
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(10000).ConfigureAwait(false);
                Logger.Information("Reaper: started..");

                var c = _consulPort > 0 ? new ConsulRestClient(_consulPort) : new ConsulRestClient();

                var lookup = new HashSet<string>();
                while (true)
                {
                    try
                    {
                        var res = await c.GetCriticalServicesAsync().ConfigureAwait(false);
                        foreach (var criticalServiceId in res)
                        {
                            if (lookup.Contains(criticalServiceId))
                            {
                                await c.DeregisterServiceAsync(criticalServiceId).ConfigureAwait(false);
                                Logger.Information("Reaper: Removing {ServiceId}", criticalServiceId);
                            }
                            else
                            {
                                lookup.Add(criticalServiceId);
                                Logger.Information("Reaper: Marking {ServiceId}", criticalServiceId);
                            }
                        }
                        //remove entries that are no longer critical
                        lookup.RemoveWhere(i => !res.Contains(i));
                    }
                    catch (Exception x)
                    {
                        Logger.Error(x, "Crashed");
                    }

                    await Task.Delay(5000).ConfigureAwait(false);
                }
            });
        }
    }
}