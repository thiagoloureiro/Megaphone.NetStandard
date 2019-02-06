using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Megaphone.Core.Util
{
    public class Obj
    {
        public string ID { get; set; }
        public string Service { get; set; }
        public List<string> Tags { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public bool EnableTagOverride { get; set; }
    }

    public class JsonResponseConsul
    {
        public static object GetJsonStructure(string body)
        {
            var lstServices = new List<ServiceInformation>();

            var idx = new List<int>();

            bool createObject = false;

            var lstStr = new List<string>();

            using (var reader = new JsonTextReader(new StringReader(body)))
            {
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        lstStr.Add(reader.Value.ToString());
                    }
                }
            }

            var aryLst = lstStr.ToArray();

            for (int i = 0; i < aryLst.Count(); i++)
            {
                if (aryLst[i] == "ID")
                    idx.Add(i);
            }

            foreach (var id in idx)
            {
                for (int i = id; i < id; i++)
                {
                }
            }

            return null;
        }
    }
}