using System;

namespace Megaphone.Core
{
    public class ServiceData
    {
        public string serviceName { get; set; }
        public string serviceId { get; set; }
        public string version { get; set; }
        public Uri _uri { get; set; }
        public string[] tags { get; set; }
    }
}