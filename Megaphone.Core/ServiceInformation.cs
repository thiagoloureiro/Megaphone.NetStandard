namespace Megaphone.Core
{
    public class ServiceInformation
    {
        public ServiceInformation(string serviceAddress, int servicePort, string id, string[] tags)
        {
            Address = serviceAddress;
            Port = servicePort;
            Tags = tags;
            Id = id;
        }

        public string Name { get; set; }
        public string Address { get; }
        public int Port { get; }
        public string[] Tags { get; }
        public string Id { get; set; }
    }
}