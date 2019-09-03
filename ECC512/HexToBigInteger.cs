using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECC512
{
    public class HexToBigInteger
    {
        public static BigInteger inputHex(string hexInput)
        {
            hexInput = hexInput.ToLower();
            BigInteger result = new BigInteger(0);
            for (var i = 0; i < hexInput.Length; i++)
            {
                result = result * 16;
                var charIndex = hexInput[i];
                switch (charIndex)
                {
                    case '0':
                        {
                            result += 0;
                        }; break;
                    case '1':
                        {
                            result += 1;
                        }; break;
                    case '2':
                        {
                            result += 2;
                        }; break;
                    case '3':
                        {
                            result += 3;
                        }; break;
                    case '4':
                        {
                            result += 4;
                        }; break;
                    case '5':
                        {
                            result += 5;
                        }; break;
                    case '6':
                        {
                            result += 6;
                        }; break;
                    case '7':
                        {
                            result += 7;
                        }; break;
                    case '8':
                        {
                            result += 8;
                        }; break;
                    case '9':
                        {
                            result += 9;
                        }; break;
                    case 'a':
                        {
                            result += 10;
                        }; break;
                    case 'b':
                        {
                            result += 11;
                        }; break;
                    case 'c':
                        {
                            result += 12;
                        }; break;
                    case 'd':
                        {
                            result += 13;
                        }; break;
                    case 'e':
                        {
                            result += 14;
                        }; break;
                    case 'f':
                        {
                            result += 15;
                        }; break;
                    default:
                        {
                            throw new Exception(charIndex.ToString());
                        }
                }
            }
            return result;
        }
    }
}
