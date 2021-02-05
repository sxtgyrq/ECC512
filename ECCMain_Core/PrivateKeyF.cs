using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class PrivateKeyF
    {
        internal static void Config()
        {
            throw new NotImplementedException();
        }

        internal static bool Check(string inputSting, out System.Numerics.BigInteger privateBigInteger)
        {
            var privateByte = Base58.Decode(inputSting);
            // var privateByte = Hex.HexToBytes32(inputSting);
            //  var privateKey1 = Calculate.Encode(privateByte);
            if (privateByte.Length == 38)
            {
                if (privateByte[0] == 0x80 && privateByte[33] == 0x01)
                {
                    var m = new List<byte>();
                    for (var i = 0; i < 34; i++)
                    {
                        m.Add(privateByte[i]);
                    }
                    byte[] chechHash = Calculate.GetCheckSum(m.ToArray());
                    if (
                        chechHash[0] == privateByte[34] &&
                        chechHash[1] == privateByte[35] &&
                        chechHash[2] == privateByte[36] &&
                        chechHash[3] == privateByte[37])
                    {
                        privateBigInteger = 0;
                        for (var i = 1; i < 33; i++)
                        {
                            privateBigInteger = privateBigInteger * 256;
                            privateBigInteger = privateBigInteger + Convert.ToInt32(privateByte[i]);
                        }
                        privateBigInteger = privateBigInteger % Secp256k1.q;
                        return true;
                    }
                    else
                    {
                        privateBigInteger = -1;
                        return false;
                    }
                }
                else
                {
                    privateBigInteger = -1;
                    return false;
                }
            }
            else if (privateByte.Length == 37)
            {
                if (privateByte[0] == 0x80)
                {
                    var m = new List<byte>();
                    for (var i = 0; i < 33; i++)
                    {
                        m.Add(privateByte[i]);
                    }
                    byte[] chechHash = Calculate.GetCheckSum(m.ToArray());
                    if (
                        chechHash[0] == privateByte[33] &&
                        chechHash[1] == privateByte[34] &&
                        chechHash[2] == privateByte[35] &&
                        chechHash[3] == privateByte[36])
                    {
                        privateBigInteger = 0;
                        for (var i = 1; i < 33; i++)
                        {
                            privateBigInteger = privateBigInteger * 256;
                            privateBigInteger = privateBigInteger + Convert.ToInt32(privateByte[i]);
                        }
                        privateBigInteger = privateBigInteger % Secp256k1.q;
                        return true;
                    }
                    else
                    {
                        privateBigInteger = -1;
                        return false;
                    }
                }
                else
                {
                    privateBigInteger = -1;
                    return false;
                }
            }
            else
            {
                privateBigInteger = -1;
                return false;
            }
        }

        internal static BigInteger[] Sign(BigInteger privateBigInteger, byte[] hashCode)
        {
            return ECCMain.Sign.GenerateSignature(privateBigInteger, hashCode);


        }

        //internal static void Adapter(ref byte[] privateByte)
        //{
        //    var result = new byte[32];
        //    var l = privateByte.Length;
        //    for (int i = 0; i < 32; i++)
        //    {
        //        if (i < l) result[i] = privateByte[i];
        //        else result[i] = Convert.ToByte(0);

        //    }
        //    privateByte = result;
        //}
    }
}
