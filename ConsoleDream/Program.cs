using ECCMain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleDream
{
    class Program
    {
        static void Main(string[] args)
        {
            //梦里梦到Apple 或者 apple 或者APPLE 加日期格式也许是某地址的种子！
            string[] seedStr = new string[] { "apple", "Apple", "APPLE" };
            DateTime start = new DateTime(2019, 1, 1, 0, 0, 0);
            DateTime end = DateTime.Now;
            while (start < end)
            {
                {
                    var dateStr = new string[] {
                        start.Date.ToString("yyyyMMdd"),
                        start.Date.ToString("yyyy-MM-dd")
                };
                    for (var i = 0; i < seedStr.Length; i++)
                    {
                        for (var j = 0; j < dateStr.Length; j++)
                        {
                            string seed1 = $"{seedStr[i]}{dateStr[j]}";
                            string seed2 = $"{dateStr[j]}{seedStr[i]}";
                            var r = getAddress(seed1);
                            var r2 = getAddress(seed2);


                        }
                    }
                }
                {
                    for (var i = 0; i < 86400; i++)
                    {
                        var t = start.Date.AddSeconds(i);
                        var timeStr = new string[]
                        {
                        t.GetDateTimeFormats('s')[0].ToString(),
                        string.Format("{0:G}", t),
                         string.Format("{0:R}", t)
                        };
                        for (var kk = 0; kk < seedStr.Length; kk++)
                        {
                            for (var j = 0; j < timeStr.Length; j++)
                            {
                                string seed1 = $"{seedStr[i]}{timeStr[j]}";
                                string seed2 = $"{timeStr[j]}{seedStr[i]}";
                                getAddress(seed1);
                                getAddress(seed2);
                            }
                        }
                    }
                }
                //dt.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25 
                // string.Format("{0:G}", dt);//2005-11-5 14:23:23 
                // string.Format("{0:R}", dt);//Sat, 05 Nov 2005 14:23:23 GMT
                //string.Format("{0:s}",dt);//2005-11-05T14:23:23 
                //string.Format("{0:d}",dt);//2005-11-5 
                // var
                //var stringA=$"Apple"
            }
            //   for (var i = start; i < end; i)
            Console.WriteLine("Hello World!");
        }

        static List<string> getAddress(string input)
        {
            List<string> result = new List<string>();
            Console.WriteLine($"输入助记词！");
            SHA256 sha256 = new SHA256Managed();
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                // var privateKey = HexToBigInteger.inputHex("e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355");

                //var inputSting = "e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355";
                //var inputSting = ;
                var privateKey = Bytes32.ConvetToBigInteger(hash);
                privateKey = privateKey % Secp256k1.q;
                var privateByte = hash;

                {
                    var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, new byte[] { 0x01 });
                    byte[] chechHash = Calculate.GetCheckSum(resultAdd);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
                    var privateKey1 = Calculate.Encode(resultAdd);
                    Console.WriteLine($"您压缩后的私钥为{privateKey1}");
                }
                {
                    var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
                    byte[] chechHash = Calculate.GetCheckSum(resultAdd);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
                    var privateKey1 = Calculate.Encode(resultAdd);
                    Console.WriteLine($"您压缩前的私钥为{privateKey1}");
                }
                var publicKey = Calculate.getPublicByPrivate(privateKey);
                if (publicKey != null)
                {
                    var walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKey);
                    Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                    var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKey);
                    Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                    result.Add(walletOfcompressed);
                    result.Add(walletOfUncompressed);
                }
            }

            {
                byte[] hash = sha256.ComputeHash(sha256.ComputeHash(Encoding.UTF8.GetBytes(input)));
                // var privateKey = HexToBigInteger.inputHex("e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355");

                //var inputSting = "e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355";
                //var inputSting = ;
                var privateKey = Bytes32.ConvetToBigInteger(hash);
                privateKey = privateKey % Secp256k1.q;
                var privateByte = hash;

                {
                    var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, new byte[] { 0x01 });
                    byte[] chechHash = Calculate.GetCheckSum(resultAdd);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
                    var privateKey1 = Calculate.Encode(resultAdd);
                    Console.WriteLine($"您压缩后的私钥为{privateKey1}");
                }
                {
                    var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
                    byte[] chechHash = Calculate.GetCheckSum(resultAdd);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
                    var privateKey1 = Calculate.Encode(resultAdd);
                    Console.WriteLine($"您压缩前的私钥为{privateKey1}");
                }
                var publicKey = Calculate.getPublicByPrivate(privateKey);
                if (publicKey != null)
                {
                    var walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKey);
                    Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                    var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKey);
                    Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                    result.Add(walletOfcompressed);
                    result.Add(walletOfUncompressed);
                }
            }

            return result;
        }
    }
}
