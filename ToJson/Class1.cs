using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToJson
{
    public class Class1
    {
        public static void Show(Dictionary<byte, Dictionary<int, byte>> originToSecretCode)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(originToSecretCode);
            Console.WriteLine(json); 
        }
    }
}
