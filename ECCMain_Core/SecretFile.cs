using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class SecretFile
    {
        public static void SecretFileF()
        {
            while (true)
            {

                Console.WriteLine($"输入助记词！");
                SHA256 sha256 =   SHA256.Create();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Console.ReadLine()));
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
                    Console.WriteLine($"您压缩后的私钥为 {privateKey1}");
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
                    var walletOfP2SH = PublicKeyF.GetAddressOfP2SH(publicKey);
                    Console.WriteLine($"P2SH钱包地址为：{walletOfP2SH}"); 
                }
                else
                {
                    Console.WriteLine($"您输入了零元！");
                }
                Console.WriteLine("E/Exit,退出当前");

                if (Console.ReadLine().ToUpper() == "E")
                {
                    break;
                }
            } 


            //Console.WriteLine($"您的非私钥为80{HexToBigInteger.bigIntergetToHex(publicKey[0])}01");


            //var publicKeyName = getPublicKeyName(publicKey);
            //Console.WriteLine($"您的公钥16进制为{publicKeyName}");
            //if (publicKey != null)
            //    Console.WriteLine($"您的公钥10进制为:{publicKey[0]},{publicKey[1]}");


            //Random rm = new Random(DateTime.Now.GetHashCode());
            //for (var i = 1; i <= 256 * 256; i++)
            //{
            //    var M = getPublicByPrivate(new BigInteger(i));
            //}
            //   throw new NotImplementedException();
        }
    }
}
