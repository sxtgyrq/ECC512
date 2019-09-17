using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class LockAndKeyRead
    {
        public static string Get(string privateKeyOfSenderPath)
        {
            string text = System.IO.File.ReadAllText(privateKeyOfSenderPath);
            return text;
        }
    }
}
