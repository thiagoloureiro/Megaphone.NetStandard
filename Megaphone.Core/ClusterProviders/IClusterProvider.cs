using Megaphone.Core.Util;
using System;
using System.Threading.Tasks;

namespace Megaphone.Core.ClusterProviders
{
    public interface IClusterProvider
    {
        Task<ServiceInformation[]> FindServiceInstancesAsync(string name);

        Task<Services[]> FindServiceByTagAsync(string[] tags);

        Task RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri);

        Task RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri, string[] tags);

        Task BootstrapClientAsync();

        Task KvPutAsync(string key, object value);

        Task<T> KvGetAsync<T>(string key);

        Task DeRegisterServiceAsync(string serviceId);
    }

    public static class ClusterProviderExtensions
    {
        public static async Task<ServiceInformation> FindServiceInstanceAsync(this IClusterProvider self, string serviceName)
        {
            var res = await self.FindServiceInstancesAsync(serviceName).ConfigureAwait(false);
            if (res.Length == 0)
                return null;

            return res[ThreadLocalRandom.Current.Next(0, res.Length)];
        }
    }
}