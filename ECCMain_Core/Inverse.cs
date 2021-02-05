using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class Inverse
    {
        public static BigInteger ex_gcd(BigInteger a, BigInteger b)
        {
            //while (a < 0)
            //{
            //    a += b;
            //}
            a = a % b;
            if (a < 0)
            {
                a += b;
            }
            BigInteger x, y;
            ex_gcd(a, b, out x, out y);
            if (x > 0)
            {
                return x;
            }
            else
            {
                return b + x;
            }
        }

        public static BigInteger ex_gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            BigInteger ret, tmp;
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            ret = ex_gcd(b, a % b, out x, out y);
            tmp = x;
            x = y;
            y = tmp - a / b * y;
            return ret;
        }
    }
}
