using System;
using System.Threading.Tasks;

namespace Megaphone.Core.ClusterProviders
{
    public class ConsulProvider : IClusterProvider
    {
        public async Task<ServiceInformation[]> FindServiceInstancesAsync(string name)
        {
            return await new ConsulRestClient().FindServiceAsync(name).ConfigureAwait(false);
        }

        public async Task RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri)
        {
            await new ConsulRestClient().RegisterServiceAsync(serviceName, serviceId, uri).ConfigureAwait(false);
        }

        public Task BootstrapClientAsync()
        {
            return Task.FromResult(0);
        }

        public async Task KvPutAsync(string key, object value)
        {
            await new ConsulRestClient().KvPutAsync(key, value).ConfigureAwait(false);
        }

        public async Task<T> KvGetAsync<T>(string key)
        {
            return await new ConsulRestClient().KvGetAsync<T>(key).ConfigureAwait(false); ;
        }
    }
}