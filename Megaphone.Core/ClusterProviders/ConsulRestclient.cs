using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Megaphone.Core.Util;

namespace Megaphone.Core.ClusterProviders
{
    public class ConsulRestClient
    {
        private readonly int _consulPort;
        private readonly string _consulHost;
        private readonly string _consulStatusFrequency;
        private readonly string _consulStatus;
        private static readonly HttpClient _httpClient = new HttpClient();

        public ConsulRestClient()
        {
            int num = 8500;
            _consulPort = 0;

            if (!int.TryParse(GlobalVariables.GetConfigurationValue("Port"), out _consulPort))
            {
                _consulPort = num;
            }

            _consulStatus = (GlobalVariables.GetConfigurationValue("StatusEndPoint") ?? "status");
            _consulStatusFrequency = (GlobalVariables.GetConfigurationValue("StatusEndPointFrequencyCheck") ?? "10s");
            _consulHost = (GlobalVariables.GetConfigurationValue("Host") ?? "localhost");
        }

        public ConsulRestClient(int port)
        {
            _consulPort = port;
        }

        public async Task RegisterServiceAsync(string serviceName, string serviceId, Uri address, string[] tags)
        {
            try
            {
                var payload = new
                {
                    ID = serviceId,
                    Name = serviceName,
                    Tags = tags,
                    Address = address.Host,
                    Port = address.Port,
                    Check = new
                    {
                        HTTP = address + _consulStatus,
                        Interval = _consulStatusFrequency,
                        DeregisterCriticalServiceAfter = "15s"
                    }
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json);

                var res = await _httpClient.PutAsync($"http://{_consulHost}:{_consulPort}/v1/agent/service/register", content).ConfigureAwait(false);

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Could not register service");
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not register service, consul server not found");
            }
        }

        public async Task RegisterServiceAsync(string serviceName, string serviceId, Uri address)
        {
            try
            {
                var payload = new
                {
                    ID = serviceId,
                    Name = serviceName,
                    Tags = new[] { $"urlprefix-/{serviceName}" },
                    Address = address.Host,
                    Port = address.Port,
                    Check = new
                    {
                        HTTP = address + _consulStatus,
                        Interval = _consulStatusFrequency,
                        DeregisterCriticalServiceAfter = "15s"
                    }
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json);

                var res = await _httpClient.PutAsync($"http://{_consulHost}:{_consulPort}/v1/agent/service/register", content).ConfigureAwait(false);

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Could not register service");
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not register service, consul server not found");
            }
        }

        public async Task<ServiceInformation[]> FindServiceAsync(string serviceName)
        {
            var response = await _httpClient.GetAsync($"http://{_consulHost}:{_consulPort}/v1/health/service/" + serviceName).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not find services");
            }

            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var res = JArray.Parse(body);

            return res.Select(entry => new ServiceInformation(entry["Service"]["Address"].Value<string>(),
                entry["Service"]["Port"].Value<int>(), null)).ToArray();
        }

        public async Task<Services[]> FindServiceByTagAsync(string[] tags)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://{_consulHost}:{_consulPort}/v1/agent/services").ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Could not find services");
                }

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var lstServices = JsonResponseConsul.GetJsonStructure(body).Services;

                return lstServices.Where(x => tags.Any(x.Tags.Contains)).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string[]> GetCriticalServicesAsync()
        {
            var response =
                await
                    _httpClient.GetAsync($"http://{_consulHost}:{_consulPort}/v1/health/state/critical").ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not get service health");
            }
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var res = JArray.Parse(body);
            return res.Cast<JObject>().Select(service => service["ServiceID"].Value<string>()).ToArray();
        }

        public async Task DeregisterServiceAsync(string serviceId)
        {
            var response =
                await
                    _httpClient.PutAsync($"http://{_consulHost}:{_consulPort}/v1/agent/service/deregister/" + serviceId, null)
                        .ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not de register service");
            }
        }

        public async Task KvPutAsync(string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json);

            var response =
                await
                    _httpClient.PutAsync($"http://{_consulHost}:{_consulPort}/v1/kv/" + key, content).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not put value");
            }
        }

        public async Task<T> KvGetAsync<T>(string key)
        {
            var response = await _httpClient.GetAsync($"http://{_consulHost}:{_consulPort}/v1/kv/" + key).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (T)Convert.ChangeType("Error: Key not found", typeof(T));
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not get value");
            }

            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializedBody = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(body);
            var bytes = Convert.FromBase64String((string)deserializedBody[0]["Value"]);
            var strValue = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(strValue);
        }
    }
}