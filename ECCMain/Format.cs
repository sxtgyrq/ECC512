using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECCMain
{
    public class Format
    {
        //  /^5[HJK] [1-9A-Za-z] [^OIl]{49}/
        public static Regex regexOfCompressBase58 = new Regex(@"^5[HJK][1-9A-HJ-NP-Za-km-z]{49}");
        ///0[xX][0-9a-fA-F]+/
        public static Regex regexOfComPressPublic64 = new Regex(@"^0[][0-9a-fA-F]{74,76}");
    }
}
