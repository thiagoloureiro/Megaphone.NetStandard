using System.Collections.Generic;

namespace Megaphone.Core
{
    public class Weights
    {
        public int Passing { get; set; }
        public int Warning { get; set; }
    }

    public class Services
    {
        public string ID { get; set; }
        public string Service { get; set; }
        public List<string> Tags { get; set; }
        public Meta Meta { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public Weights Weights { get; set; }
        public bool EnableTagOverride { get; set; }
    }

    public class ServiceInformationExtended
    {
        public List<Services> Services { get; set; }
    }

    public class Meta
    {
    }
}