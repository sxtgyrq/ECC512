using NBitcoin;
using System.Security.Cryptography;

namespace ECCMain
{
    public class CLTV
    {
        private static readonly RIPEMD160Managed ripemd160 = new RIPEMD160Managed();
        private static readonly SHA256 sha256 = SHA256.Create();
        public static void CLTVF()
        {
            Console.WriteLine($"以下是脚本样例");
            Console.WriteLine($"OP_PUSHDATA1 04 1546288031 OP_CHECKLOCKTIMEVERIFY OP_DROP OP_DUP OP_HASH160 371c20fb2e9899338ce5e99908e64fd30b789313 OP_EQUALVERIFY OP_CHECKSIG");
            Console.WriteLine($"以下是脚本格式");
            Console.WriteLine($"OP_PUSHDATA1 0x04 <locktime> OP_CHECKLOCKTIMEVERIFY OP_DROP OP_DUP OP_HASH160 0x14 <Your Public Key Hash> OP_EQUALVERIFY OP_CHECKSIG");
            Console.WriteLine($"输入locktime，即赎回的区块高度");
            Console.WriteLine($"数值格式：locktime 是一个32位无符号整数。它可以直接设为你想要的区块高度的数值。");
            Console.WriteLine($"如果 locktime 的值小于 500000000，它被解释为区块高度。");
            Console.WriteLine($"这里，默认其值必须小于 500000000");
            Console.WriteLine($"例如，如果你希望交易在区块高度为 700000 的时候才能被处理，你就将 locktime 设为 700000。");
            Console.WriteLine($"请输入 locktime 的值");
            var inputValue = Console.ReadLine();
            if (inputValue != null)
            {
            }
            else return;
            UInt32 lockTime = UInt32.Parse(inputValue);

            Console.WriteLine($"输入私钥");
            Console.WriteLine($"依据私钥会生成 OP_HASH160");
            inputValue = Console.ReadLine();
            if (inputValue != null)
            {
            }
            else return;
            string privateKeyStr = inputValue;

            System.Numerics.BigInteger privateKeyBigInteger;
            if (PrivateKeyF.Check(privateKeyStr, out privateKeyBigInteger))
            {
                var publicKey = Calculate.getPublicByPrivate(privateKeyBigInteger);

                var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
                HexToByteArray.ChangeDirection(ref publicKeyArray1);
                // var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);

                byte[] resultAdd;
                if (publicKey[1].IsEven)
                    resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x02 }, publicKeyArray1);
                else
                    resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x03 }, publicKeyArray1);

                //if (show)
                //{ }
                //Console.WriteLine($"压缩公钥为{ Hex.BytesToHex(resultAdd)}");
                //   Console.WriteLine($"压缩公钥为{ Calculate.Encode(resultAdd)}");

                var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));


                if (step3.Length != 20)
                {
                    throw new Exception($" ripemd160.ComputeHash(sha256.ComputeHash(resultAdd)) result.Length != 20");
                }
                //   byte[] locktimeBytes = BitConverter.GetBytes(lockTime);

                // if (BitConverter.IsLittleEndian)
                //   Array.Reverse(locktimeBytes);  // 确保是大端序
                uint operatelockTime = lockTime;
                List<byte> script = new List<byte>();
                script.Add(0x4c); // OP_PUSHDATA1 76
                script.Add(0x4); // 长度为4
                {
                    /*
                     * 在比特币脚本中，当您使用 locktime 值，如数字 1，
                     * 并且它占用四个字节时，这个值在脚本中以小端序格
                     * 式表示。小端序意味着最低有效字节在前。因此，数
                     * 字 1 表示为四个字节时，将以如下格式出现：
                     * 01 00 00 00
                     * 数字 257 以四字节的小端序格式表示时会是这样的：
                     * 01 01 00 00
                     */
                    for (int indexOfLoop = 0; indexOfLoop < 4; indexOfLoop++)
                    {
                        script.Add(Convert.ToByte(operatelockTime % 256));
                        operatelockTime /= 256;
                    }
                }
                //script.AddRange(locktimeBytes);             // 添加 locktime
                script.Add(0xb1);                           // OP_CHECKLOCKTIMEVERIFY 177
                script.Add(0x75);                           // OP_DROP
                script.Add(0x76);                           // OP_DUP
                script.Add(0xa9);                           // OP_HASH160
                script.Add(0x14);                           // 长度为20
                script.AddRange(step3);        // 添加RIPEMD-160 哈希
                script.Add(0x88);                           // OP_EQUALVERIFY
                script.Add(0xac);                           // OP_CHECKSIG

                var step4 = script.ToArray();
                var step5 = sha256.ComputeHash(step4);

                //   var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });
                var step6 = ripemd160.ComputeHash(step5);
                var step7 = Calculate.BiteSplitJoint(new byte[] { 0x05 }, step6);
                // Console.WriteLine($"step7:{Hex.BytesToHex(step7)}");
                var step8 = sha256.ComputeHash(sha256.ComputeHash(step7));
                var step9 = Calculate.BiteSplitJoint(step7, new byte[] { step8[0], step8[1], step8[2], step8[3] });
                var addr = Calculate.Encode5(step9);

                Console.WriteLine($"协议地址为：{addr}");

                OtherBitCoinLibrary.GenerateHex.GetAddr(privateKeyStr, lockTime);
            }

        }

        public static void Cost()
        {
            Console.WriteLine($"依据私钥会生成 OP_HASH160");
            var inputValue = Console.ReadLine();
            if (inputValue != null)
            {
            }
            else return;
            string privateKeyStr = inputValue;
            Console.WriteLine($"https://blockchain.info/q/getblockcount  可以获得当前区块值");
            Console.WriteLine($"请输入 locktime 的值");
            inputValue = Console.ReadLine();
            if (inputValue != null)
            {
            }
            else return;
            UInt32 lockTime = UInt32.Parse(inputValue);

            OtherBitCoinLibrary.CLTV.GetHex4(privateKeyStr, lockTime); 
        }
        public static void CLTVOutF() { }

        public static void Test()
        {
            OtherBitCoinLibrary.CLTV.Test();// TestCLTV();
                                            // throw new NotImplementedException();
        }
    }
    //CLTV
}
