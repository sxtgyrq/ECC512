using System;
using System.Security.Cryptography;
using System.Text;

namespace ECCMain
{
    public class Produce10000
    {
        public static void Produce10000F()
        {
            Console.WriteLine($"输入助记词！");
            var word = Console.ReadLine();
            for (int i = 0; i < 10000; i++)
            {
                //Console.WriteLine($"输入助记词！");
                var secretWords = $"{word}{i.ToString("D4")}";
                Console.WriteLine(secretWords);
                SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretWords));
                // var privateKey = HexToBigInteger.inputHex("e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355");

                //var inputSting = "e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355";
                //var inputSting = ;
                var privateKey = Bytes32.ConvetToBigInteger(hash);
                privateKey = privateKey % Secp256k1.q;
                var privateByte = hash;
                string privateKey1;
                {
                    var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x80 }, privateByte);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, new byte[] { 0x01 });
                    byte[] chechHash = Calculate.GetCheckSum(resultAdd);
                    resultAdd = Calculate.BiteSplitJoint(resultAdd, chechHash);
                    privateKey1 = Calculate.Encode(resultAdd);
                    // Console.WriteLine($"您压缩后的私钥为{privateKey1}");
                }
                var publicKey = Calculate.getPublicByPrivate(privateKey);
                string walletOfcompressed;
                if (publicKey != null)
                {
                    walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKey);
                    // Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                    Console.WriteLine($"{i}_{privateKey1}_{walletOfcompressed}");
                    File.WriteAllText($"A{i.ToString("D4")}.txt", walletOfcompressed);
                }
                else
                {
                    Console.WriteLine($"由于错误，暂停！");
                    Console.ReadLine();
                }

            }

            Console.WriteLine($"运算完毕！");
            Console.ReadLine();
        }
    }
}
