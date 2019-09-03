using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
   public class Base58
    {
        private const string Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        public static byte[] Decode(string base58)
        {
            // Decode Base58 string to BigInteger 
            BigInteger intData = 0;
            for (int i = 0; i < base58.Length; i++)
            {
                int digit = Digits.IndexOf(base58[i]); //Slow
                if (digit < 0)
                    throw new FormatException(string.Format("Invalid Base58 character `{0}` at position {1}", base58[i], i));
                intData = intData * 58 + digit;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            int leadingZeroCount = base58.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
                intData.ToByteArray()
                .Reverse()// to big endian
                .SkipWhile(b => b == 0);//strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
            return result;
        }
    }
}
