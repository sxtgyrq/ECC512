using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        [STAThread]   //加这个属性，创建并进入单元
        static void Main(string[] args)
        {
            //Console.WriteLine(new BigInteger(new byte[] { 22, 2, 3 }));
            Console.WriteLine($"版本号1.1");
            Console.WriteLine(ECCMain.Secp256k1.p % 4);
            Console.WriteLine((ECCMain.Secp256k1.p + 1) / 4);
            while (true)
            {
                Console.WriteLine("此程序采用Secp256k1，加密算法，用于文件加密、解密、比特币交易等程序！");
                Console.WriteLine("G查看！S,加密；D解密；CHECKONLY；SIGN；GENERATE50000ADDRESS");
                var command = Console.ReadLine();
                command = command.ToUpper();
                switch (command)
                {
                    case "G":
                        {
                            ECCMain.SecretFile.SecretFileF();
                        }; break;
                    case "S":
                        {

                            ECCMain.PublicKeyComPress.ComPress(
                                () => Console.ReadLine(),
                                (a) => Console.WriteLine(a));
                        }; break;
                    case "SS":
                        {

                            ECCMain.PublicKeyComPress.ComPressFiles(() => Console.ReadLine(),
                                (a) => Console.WriteLine(a));
                        }; break;
                    case "D":
                        {
                            //deciphering;
                            //解密
                            ECCMain.Deciphering.Decrypt();
                        }; break;
                    case "DD":
                        {
                            //deciphering;
                            //解密
                            ECCMain.Deciphering.DecryptFiles();
                        }; break;
                    case "T":
                        {
                            //bool s;
                            //var p = EditWin.GetSavePath.Get(out s);
                            //Console.WriteLine($"{p}");
                        }; break;
                    case "SIGN":
                        {
                            ECCMain.Sign.SignMessage();
                        }; break;
                    case "VERIFY":
                        {

                            var a = "HGckL6gXSN4BgS4dg8edS2R7kA5GK/34nFDLm4bqS/gUvN/lPL/uIc55jnLQ3N5qF4L8wyFKQi3RJFMNVdStxl8=";
                            var b = "啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊140啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊140";
                            var c = ECCMain.Sign.verify_message(a, b, 0);
                            Console.WriteLine($"{c}");
                            Console.WriteLine(ECCMain.Sign.verify_message(Console.ReadLine(), Console.ReadLine(), 0));
                        }; break;
                    //case "N":
                    //    {
                    //        Console.WriteLine("非压缩算法没有");
                    //    }; break;
                    case "E":
                        {
                            return;
                        }; ;
                    case "GET":
                        {
                            ECCMain.GetByName.Get();
                        }; break;
                    case "CHECKONLY":
                        {
                            CheckOnly.Check();
                        }; break;
                    case "GENERATE50000ADDRESS": 
                        {
                            ECCMain.Produce10000.Produce10000F();
                        };break;
                    default:
                        {

                        }; break;
                }
            }
            //var c = ECCMain.HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3");
            //  while (true)
            {
                //var result = get(ECCMain.HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3"));
                //var result = get(123456);
                //for (int i = 0; i < result.Length; i++)
                //{
                //    Console.Write(result[i] ? "□" : "■");
                //}
                //var xx = new System.Numerics.BigInteger(-1);
                //var xxxx = xx % p;
                //Console.WriteLine($"{xxxx}");
                //var c = 13;
                //string percentV = "";
                //for (int i = 1; i <= 256 * 256 * 256; i++)
                //{
                //    var r = ECCMain.Inverse.ex_gcd(i, p);
                //    //Console.WriteLine($"{i}");

                //    var percentItemV = $"{ i * 100 / (256 * 256 * 256)}%";
                //    if (percentItemV != percentV)
                //    {
                //        Console.WriteLine(percentV);
                //        percentV = percentItemV;
                //    }
                //    //Console.WriteLine($"{i},{r},{i}×{r}={(i * r) / c}×{c}+1;");
                //}
                //Console.WriteLine($"加载解压项运算完毕");

                //Console.WriteLine($"T/Test");
                //var command = Console.ReadLine();
                //command = command.ToUpper();

                //switch (command)
                //{
                //    //case "T":
                //    //    {
                //    //        while (true)
                //    //        {
                //    //            Test();
                //    //        }
                //    //        continue;
                //    //    }; break;
                //    case "F":
                //        {

                //        }; break;
                //    case "Y":
                //        {
                //            Compress();
                //            Console.ReadLine();
                //        }; break;
                //}
            }
        }

        private static void SecretFile()
        {
            throw new NotImplementedException();
        }

        private static void Compress()
        {
            while (true)
                getKeys();
            return;
            Console.WriteLine("请输入文件名");
            var fileName = Console.ReadLine();
            string filename = System.IO.Path.GetFileName(fileName);//文件名 “Default.aspx”
            string extension = System.IO.Path.GetExtension(fileName);
            string pointV = string.IsNullOrEmpty(extension.Trim()) ? "" : ".";
            string fileNameSave = $"{fileName}{pointV}{extension}";
            var fileNameData = UnicodeEncoding.UTF8.GetBytes(fileNameSave);
            long fileNameDataLength = fileNameData.LongLength;

            getKeys();

            using (FileStream fs = new FileStream(filename, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
            {
                long length = fs.Length;
                var leftLength = length % 3;
                var loopLength = length / 3;

                for (var i = 0; i < length - 3; i += 3) { }

                using (BinaryReader br = new BinaryReader(fs))
                {

                    for (long i = 0; i < loopLength; i++)
                    {

                    }
                    //br.ReadByte()
                    //br.ReadBytes(startPosition);
                    //var bytes = br.ReadBytes(endPosition - startPosition);
                    //fs.Close();
                    //return bytes;
                }

            }
            //  throw new NotImplementedException();

            Console.WriteLine($"filename:{filename},extension:{extension}");

            //1024*1024 /256=4*1024B =4KB  1024*1024 最大值   256*256*256
        }

        private static void getKeys()
        {
            Console.WriteLine($"输入私钥！");
            var privateKey = BigInteger.Parse(Console.ReadLine());
            var publicKey = getPublicByPrivate(privateKey);
            var publicKeyName = getPublicKeyName(publicKey);
            Console.WriteLine($"您的公钥16进制为{publicKeyName}");
            if (publicKey != null)
                Console.WriteLine($"您的公钥10进制为:{publicKey[0]},{publicKey[1]}");


            Random rm = new Random(DateTime.Now.GetHashCode());
            for (var i = 1; i <= 256 * 256; i++)
            {
                var M = getPublicByPrivate(new BigInteger(i));
            }
            //string percentV = "";
            //for (int i = 1; i <= 256 * 256; i++)
            //{
            //    var r = ECCMain.Inverse.ex_gcd(i, p);
            //    //Console.WriteLine($"{i}");

            //    var percentItemV = $"{ i * 100 / (256 * 256 * 256)}%";
            //    if (percentItemV != percentV)
            //    {
            //        Console.WriteLine(percentV);
            //        percentV = percentItemV;
            //    }
            //    //Console.WriteLine($"{i},{r},{i}×{r}={(i * r) / c}×{c}+1;");
            //}
            //Console.WriteLine($"加载解压项运算完毕");
        }

        private static string getPublicKeyName(BigInteger[] publicKey)
        {
            if (publicKey == null)
            {
                return "Ο";
            }
            else
            {
                return $"0x{ECCMain.HexToBigInteger.bigIntergetToHex(publicKey[0])},0x{ECCMain.HexToBigInteger.bigIntergetToHex(publicKey[1])}";
            }
        }

        static System.Numerics.BigInteger p = new BigInteger(17);
        static System.Numerics.BigInteger a = new BigInteger(2);
        static System.Numerics.BigInteger b = new BigInteger(2);
        static System.Numerics.BigInteger x = new BigInteger(5);
        static System.Numerics.BigInteger y = new BigInteger(1);
        static System.Numerics.BigInteger q = new BigInteger(19);

        //static System.Numerics.BigInteger p = ECCMain.HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3");
        //static System.Numerics.BigInteger a = ECCMain.HexToBigInteger.inputHex("7830A3318B603B89E2327145AC234CC594CBDD8D3DF91610A83441CAEA9863BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117A72BF2C7B9E7C1AC4D77FC94CA");
        //static System.Numerics.BigInteger b = ECCMain.HexToBigInteger.inputHex("3DF91610A83441CAEA9863BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117A72BF2C7B9E7C1AC4D77FC94CADC083E67984050B75EBAE5DD2809BD638016F723");
        //static System.Numerics.BigInteger x = ECCMain.HexToBigInteger.inputHex("81AEE4BDD82ED9645A21322E9C4C6A9385ED9F70B5D916C1B43B62EEF4D0098EFF3B1F78E2D0D48D50D1687B93B97D5F7C6D5047406A5E688B352209BCB9F822");
        //static System.Numerics.BigInteger y = ECCMain.HexToBigInteger.inputHex("7DDE385D566332ECC0EABFA9CF7822FDF209F70024A57B1AA000C55B881F8111B2DCDE494A5F485E5BCA4BD88A2763AED1CA2B2FA8F0540678CD1E0F3AD80892");
        //static System.Numerics.BigInteger q = ECCMain.HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA70330870553E5C414CA92619418661197FAC10471DB1D381085DDADDB58796829CA90069");

        private static void Test()
        {
            //var p = ECCMain.HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3");
            Random rm = new Random(DateTime.Now.GetHashCode());
            int hash = rm.Next(0, int.MaxValue);
            var privateKey = (new System.Numerics.BigInteger(rm.Next(0, int.MaxValue))) * (new System.Numerics.BigInteger(rm.Next(0, int.MaxValue))) * (new System.Numerics.BigInteger(rm.Next(0, int.MaxValue))) * (new System.Numerics.BigInteger(rm.Next(0, int.MaxValue)));

            privateKey = privateKey % p;

            //var A = ECCMain.HexToBigInteger.inputHex("7830A3318B603B89E2327145AC234CC594CBDD8D3DF91610A83441CAEA9863BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117A72BF2C7B9E7C1AC4D77FC94CA");
            //var B = ECCMain.HexToBigInteger.inputHex("3DF91610A83441CAEA9863BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117A72BF2C7B9E7C1AC4D77FC94CADC083E67984050B75EBAE5DD2809BD638016F723");

            BigInteger[] doubleP = new System.Numerics.BigInteger[] { x, y };
            int k = 1;
            bool isZero = false;
            while (!isZero)
            {
                //Console.WriteLine($"{k},{k % 19}×G={doubleP[0]},{doubleP[1]}");
                doubleP = getDoubleP(doubleP, out isZero);
                k = k * 2;
                if (k >= 1900000)
                {
                    break;
                }
            }


            BigInteger[] result = null;
            BigInteger[] baseP = new System.Numerics.BigInteger[] { x, y };
            Console.WriteLine("输入整数");
            var xx = Console.ReadLine();
            BigInteger startIndex;
            if (xx == "q")
            {
                Console.WriteLine($"{q.ToString()}");
                startIndex = q;
            }
            else
            {
                startIndex = BigInteger.Parse(xx); ;
            }
            var r = get(startIndex);
            //bool isZero = false;
            for (int i = 0; i < r.Length; i++)
            {
                if (!r[i])
                {
                    if (baseP == null)
                    {
                    }
                    else
                    {
                        if (result == null)
                        {
                            result = baseP;
                        }
                        else
                        {
                            result = pointPlus(result, baseP, out isZero);

                            //Console.WriteLine($"baseP{i} ={baseP[0]},{baseP[1]}");
                        }
                        //  result = result == null ? baseP : pointPlus(result, baseP); 
                    }
                }
                baseP = getDoubleP(baseP, out isZero);

            }



            if (result == null)
            {
                Console.WriteLine($"结果为空元");
            }
            else
            {
                Console.WriteLine($"{startIndex},{startIndex % q}×G={result[0]},{result[1]}");
            }

            //result = pointPlus(new BigInteger[] { 10, 6 }, new BigInteger[] { 7, 6 }, out isZero);
            //Console.WriteLine($"result  ={result[0]},{result[1]}");
            //result = pointPlus(result, new BigInteger[] { 10, 11 }, out isZero);
            //Console.WriteLine($"result  ={result[0]},{result[1]}");
            //result = pointPlus(result, new BigInteger[] { 13, 7 }, out isZero);
            //Console.WriteLine($"result  ={result[0]},{result[1]}");
            //BigInteger[] itemP = new System.Numerics.BigInteger[] { x, y };
            //BigInteger[] baseP = new System.Numerics.BigInteger[] { x, y };
            //int kk = 1;
            //bool isZero = false;
            //do
            //{
            //    Console.WriteLine($"{kk}×1={itemP[0]},{itemP[1]}");

            //    itemP = pointPlus(itemP, baseP, out isZero);
            //    kk++;
            //}
            //while (!isZero);
            //int k = 0;
            //while (!(doubleP[0] - a).IsZero)
            //{
            //    doubleP = getDoubleP(doubleP);
            //    k++;
            //    if (k % 1000 == 0)
            //        Console.WriteLine($"{k}");
            //}
            // var privateKey
            //  int hash=Random 
            //   throw new NotImplementedException();
            //Console.WriteLine($"按Enter键继续");
            //Console.ReadLine();
        }

        private static BigInteger[] getDoubleP(BigInteger[] bigIntegers, out bool isZero)
        {
            var x = bigIntegers[0];
            var y = bigIntegers[1];
            //if ((y%17.IsZero)
            //{
            //    isZero = true;
            //    return null;
            //}
            //else
            {
                var s = ((3 * x * x + a) * (ECCMain.Inverse.ex_gcd((2 * y) % p, p))) % p;
                var Xr = (s * s - 2 * x) % p;
                while (Xr < 0)
                {
                    Xr += p;
                }
                //Xr = Xr % p;
                var Yr = (s * (x - Xr) - y) % p;
                while (Yr < 0)
                {
                    Yr += p;
                }
                isZero = false;
                return new BigInteger[] { Xr, Yr };
            }
        }

        private static BigInteger[] pointPlus(BigInteger[] point_P, BigInteger[] point_Q, out bool isZero)
        {
            // throw new Exception("");
            if (((point_P[0] - point_Q[0]) % p).IsZero)
            {
                if ((point_P[1] - point_Q[1] % p).IsZero)
                {
                    return getDoubleP(point_P, out isZero);
                }
                else
                {
                    isZero = true;
                    return null;
                }
            }
            else
            {
                isZero = false;
                var s = (point_P[1] - point_Q[1]) * (ECCMain.Inverse.ex_gcd((point_P[0] - point_Q[0] + p) % p, p));
                s = s % p;
                if (s < 0)
                {
                    s += p;
                }
                var Xr = (s * s - (point_P[0] + point_Q[0])) % p;

                var Yr = (s * (point_P[0] - Xr) - point_P[1]) % p;
                while (Xr < 0)
                {
                    Xr += p;
                }
                while (Yr < 0)
                {
                    Yr += p;
                }
                return new BigInteger[] { Xr, Yr };
            }
            //  var s=a[]
            //var x = bigIntegers[0];
            //var y = bigIntegers[1];
            //var s = ((3 * x * x + a) * (ECCMain.Inverse.ex_gcd(2 * y, p))) % p;
            //var Xr = (s * s - 2 * x) % p;
            //while (Xr < 0)
            //{
            //    Xr += p;
            //}
            ////Xr = Xr % p;
            //var Yr = (s * (x - Xr) - y) % p;
            //while (Yr < 0)
            //{
            //    Yr += p;
            //}
            //return new BigInteger[] { Xr, Yr };
        }

        private static BigInteger[] getPublicByPrivate(BigInteger privateKey)

        {
            if (privateKey > 0)
            {
                bool isZero;
                BigInteger[] result = null;
                BigInteger[] baseP = new System.Numerics.BigInteger[] { x, y };
                privateKey = privateKey % q;
                var r = get(privateKey);
                //bool isZero = false;
                for (int i = 0; i < r.Length; i++)
                {
                    if (!r[i])
                    {
                        if (baseP == null)
                        {
                        }
                        else
                        {
                            if (result == null)
                            {
                                result = baseP;
                            }
                            else
                            {

                                result = pointPlus(result, baseP, out isZero);

                                //Console.WriteLine($"baseP{i} ={baseP[0]},{baseP[1]}");
                            }
                            //  result = result == null ? baseP : pointPlus(result, baseP); 
                        }
                    }
                    baseP = getDoubleP(baseP, out isZero);

                }
                return result;
            }
            else
            {
                throw new Exception("privateKey的值不能为0和负数");
            }
        }

        private static bool[] get(BigInteger bigIntegers)
        {
            List<bool> result = new List<bool>();
            while (!bigIntegers.IsZero)
            {
                if (bigIntegers.IsEven)
                {
                    result.Add(true);
                }
                else
                {
                    result.Add(false);
                }
                bigIntegers = bigIntegers / 2;
            }
            return result.ToArray();
        }
    }
}
