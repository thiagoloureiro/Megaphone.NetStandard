namespace Megaphone.Core
{
    public class ServiceInformation
    {
        public ServiceInformation(string serviceAddress, int servicePort, string[] tags)
        {
            Address = serviceAddress;
            Port = servicePort;
            Tags = Tags;
        }

        public string Name { get; set; }
        public string Address { get; }
        public int Port { get; }
        public string[] Tags { get; }
    }
}