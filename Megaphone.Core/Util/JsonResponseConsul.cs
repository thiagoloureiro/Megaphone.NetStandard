using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megaphone.Core.Util
{
    public class JsonResponseConsul
    {
        public static ServiceInformationExtended GetJsonStructure(string body)
        {
            List<string> lstStr = body.Split(new[] { "\n" }, StringSplitOptions.None)
                .ToList();

            lstStr[0] = "{ \"Services\":[{";
            lstStr[1] = ""; // remove root

            for (int i = 0; i < lstStr.Count; i++)
            {
                if (lstStr[i].Contains("EnableTagOverride"))
                {
                    if (lstStr.Count - i > 10)
                    {
                        lstStr[i + 1] = "}, {";
                        lstStr[i + 2] = ""; // remove root
                    }
                    else
                    {
                        lstStr[i + 1] = "";
                    }
                }
            }

            lstStr[lstStr.Count - 1] = "]}";

            var str = new StringBuilder();

            lstStr.ForEach(i => str.AppendLine(i));

            var obj = JsonConvert.DeserializeObject<ServiceInformationExtended>(str.ToString());

            return obj;
        }
    }
}