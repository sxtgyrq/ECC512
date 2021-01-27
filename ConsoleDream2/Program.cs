using ECCMain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDream2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入年");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("输入月");
            int month = int.Parse(Console.ReadLine());
            Console.WriteLine("输入日");
            int day = int.Parse(Console.ReadLine());
            Thread th = new Thread(() => mainThread(year, month, day));
            th.Start();
            while (true)
                if (Console.ReadLine() == "exit")
                {
                    break;
                };

        }

        private static async void mainThread(int year, int month, int day)
        {
            string[] seedStr = new string[] { "apple", "Apple", "APPLE" };
            DateTime start = new DateTime(year, month, day, 0, 0, 0);
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

                            await Program.CheckFromUrl(r);
                            await Program.CheckFromUrl(r2);
                        }
                    }
                }
                //{
                //    for (var i = 0; i < 86400; i++)
                //    {
                //        var t = start.Date.AddSeconds(i);
                //        var timeStr = new string[]
                //        {
                //        t.GetDateTimeFormats('s')[0].ToString(),
                //         t.ToString("yyyy-MM-dd HH:mm:ss"),
                //         t.ToString("yyyyMMddHHmmss"),
                //         string.Format("{0:R}", t)
                //        };
                //        for (var kk = 0; kk < seedStr.Length; kk++)
                //        {
                //            for (var j = 0; j < timeStr.Length; j++)
                //            {
                //                string seed1 = $"{seedStr[kk]}{timeStr[j]}";
                //                string seed2 = $"{timeStr[j]}{seedStr[kk]}";
                //                var r = getAddress(seed1);
                //                var r2 = getAddress(seed2);

                //                await CheckFromUrl(r);
                //                await CheckFromUrl(r2);
                //            }
                //        }
                //    }
                //}
                File.AppendAllText("chechLog.txt", $"{start.ToString("yyyyMMdd")}" + Environment.NewLine);
                start = start.Date.AddDays(1);
            }
            //   for (var i = start; i < end; i)
            Console.WriteLine("Hello World!");
        }

        static List<string> getAddress(string input)
        {
            List<string> result = new List<string>();
            Console.WriteLine($"输入了助记词{input}");
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

        public static async Task CheckFromUrl(List<string> r)
        {
            for (var i = 0; i < r.Count; i++)
            {
                var address = $"https://blockchain.info/address/{r[i]}?format=json&s=2";
                using (HttpClient hc = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var result = await hc.GetStringAsync(address);
                    Console.WriteLine($"获取到的结果：{result}");
                    if (result.Contains("final_balance\":0,"))
                    {
                        Console.WriteLine($"{r[i]}没钱---,");
                        continue;
                    }
                    else
                    {
                        File.WriteAllText(r[i] + ".txt", "这个有结果");
                        Console.WriteLine($"{r[i]}有钱，真高兴");
                        Console.ReadLine();
                    }
                }
            }

        }
    }
}
