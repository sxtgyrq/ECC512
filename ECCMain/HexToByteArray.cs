using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class HexToByteArray
    {
        public static byte[] BigIntegerToByteArray(BigInteger inputV)
        {
            List<byte> result = new List<byte>();
            do
            {
                var v = HexToBigInteger.getInt(inputV % 256);
                result.Add(Convert.ToByte(v));
                inputV = inputV / 256;
            } while (!inputV.IsZero);

            //while (result.Count < 32)
            //{
            //    result.Insert(0, 0);
            //}

            return result.ToArray();

        }
        public static byte[] BigIntegerTo32ByteArray(BigInteger inputV)
        {
            List<byte> result = new List<byte>();
            do
            {
                var v = HexToBigInteger.getInt(inputV % 256);
                result.Add(Convert.ToByte(v));
                inputV = inputV / 256;
            } while (!inputV.IsZero);

            //while (result.Count < 32)
            //{
            //    result.Insert(0, 0);
            //}
            while (result.Count < 32)
            {
                result.Add(Convert.ToByte(0));
            }
            return result.ToArray();

        }

        internal static void ChangeDirection(ref byte[] array)
        {
            var l = array.Length;
            var result = new byte[l];
            for (var i = 0; i < l; i++)
            {
                result[i] = array[l - 1 - i];
            }
            array = result;
        }
    }
}
