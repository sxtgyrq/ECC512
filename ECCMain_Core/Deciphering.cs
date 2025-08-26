using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECCMain
{
    public class Deciphering
    {
        public static void Decrypt()
        {
            bool isRight;
            //Console.WriteLine($"输入私钥！");
            // var privateKey = HexToBigInteger.inputHex("e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355");

            //var inputSting = "e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355";
            //var inputSting = Console.ReadLine();
            //var privateKey = Hex.HexToBigInteger(inputSting);
            //privateKey = privateKey % Secp256k1.q;
            //var privateByte = Hex.HexToBytes32(inputSting);

            ////压缩公钥和原始数据的对照表
            //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();

            //Dictionary<string, byte> realInfo = new Dictionary<string, byte>();
            //
            Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();

            //密码→原码对照表，int，表征sheetNum
            Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

            //用于排序
            List<PublicKeyComPress.InfomationClass> materialForOrder = new List<PublicKeyComPress.InfomationClass>();
            //
            for (var i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
            {
                var M = Calculate.getPublicByPrivate(i + 1);
                var sM = PublicKeyComPress.ComPressPublic(M);
                //bool isRight;
                Deciphering.GetXYByByte33(sM, out isRight);
                if (isRight) { }
                else
                {
                    Console.WriteLine($"sM压缩失败！");
                    return;
                }
                var infomationOfOriginalText = Hex.BytesToHex(sM);
                materialForOrder.Add(new PublicKeyComPress.InfomationClass()
                {
                    infomationOfOriginalText = infomationOfOriginalText,
                    step = i / 256,
                    responRealByte = Convert.ToByte(i % 256),
                });
                originalTextToSecretText.Add(infomationOfOriginalText, "");

                //  materialForOrder.Add(key);
                //var infomation = $"{ Hex.BytesToHex(s1)}__{ Hex.BytesToHex(s2)}";
            }
            Dictionary<string, string> materialToIndex = new Dictionary<string, string>();

            SHA256 sha256 = new SHA256Managed();
            //bool isRight;
            //byte[] fileBinary;
            //byte[] bytes;

            {
                Console.WriteLine($"输入文件名，拖入文件即可");

                string itemPath = Console.ReadLine(); ;
                string fileDic = System.IO.Path.GetDirectoryName(itemPath);
                {
                    byte[] hash1, r, s, publicKeyS1;
                    long lengthOfFs;
                    using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        lengthOfFs = fs.Length;
                        if (lengthOfFs > 130)
                        {
                            fs.Seek(-129, SeekOrigin.End);
                            hash1 = new byte[32];
                            fs.Read(hash1, 0, 32);

                            r = new byte[32];
                            fs.Seek(-97, SeekOrigin.End);
                            fs.Read(r, 0, 32);

                            s = new byte[32];
                            fs.Seek(-65, SeekOrigin.End);
                            fs.Read(s, 0, 32);

                            publicKeyS1 = new byte[33];
                            fs.Seek(-33, SeekOrigin.End);
                            fs.Read(publicKeyS1, 0, 33);

                            List<Byte> bytes = new List<byte>();
                            for (var i = 0; i < 129; i++)
                            {
                                bytes.Add(0);
                            }
                            fs.Seek(-129, SeekOrigin.End);
                            fs.Write(bytes.ToArray(), 0, 129);

                        }
                        else
                        {
                            Console.WriteLine($"文件长度不够！！！");
                            return;
                        }
                    }

                    byte[] hashCode;
                    using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                    {
                        hashCode = sha256.ComputeHash(fs);
                    }
                    using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var length = fs.Length;
                        if (length > 130)
                        {
                            fs.Seek(-129, SeekOrigin.End);
                            fs.Write(hash1, 0, 32);

                            fs.Seek(-97, SeekOrigin.End);
                            fs.Write(r, 0, 32);

                            fs.Seek(-65, SeekOrigin.End);
                            fs.Write(s, 0, 32);

                            fs.Seek(-33, SeekOrigin.End);
                            fs.Write(publicKeyS1, 0, 33);

                        }
                        else
                        {
                            Console.WriteLine($"文件长度不够！！！");
                            return;
                        }
                    }
                    if (Bytes32.ByteArrayEqual(hash1, hashCode))
                    {
                        Console.WriteLine($"文件哈希验证成功");

                    }
                    else
                    {
                        Console.WriteLine($"文件哈希验证不成功,输入y/Y表示继续");
                        var y = Console.ReadLine().Trim();
                        if (y.ToLower() == "y") { }
                        else
                        {
                            return;
                        }
                    }
                    var rNum = Bytes32.ConvetToBigInteger(r);
                    var sNum = Bytes32.ConvetToBigInteger(s);

                    var publicKeyC1 = GetXYByByte33(publicKeyS1, out isRight);
                    if (!isRight)
                    {
                        Console.WriteLine($"解压时，解析错误！");
                        return;
                    }
                    // walletOfcompressed;
                    //string notify
                    if (Sign.VerifySignature(publicKeyC1, hashCode, rNum, sNum))
                    {
                        string walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKeyC1);
                        //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                        var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyC1);
                        //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                        Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                    }
                    else
                    {
                        Console.WriteLine($"校验成功，签名不成功！输入y/Y表示继续；");
                        //return; 
                        var y = Console.ReadLine().Trim();
                        if (y.ToLower() == "y") { }
                        else
                        {
                            return;
                        }
                    }

                    using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                    {
                        long position = 0;
                        {
                            var titleArray = new byte[32];
                            dealWithData(fs, ref position, titleArray);

                            var Title = ASCIIEncoding.ASCII.GetString(titleArray);
                            if (MainParameter.Title == Title)
                            {
                                Console.WriteLine(MainParameter.Title);
                            }
                            else
                            {
                                Console.WriteLine($"文件头错误！");
                                return;
                            }
                        }
                        {
                            var editionArray = new byte[4];
                            dealWithData(fs, ref position, editionArray);
                            byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                            if (Bytes32.ByteArrayEqual(edition, editionArray))
                            {
                                Console.WriteLine("版本检验正确！！！");
                            }
                            else
                            {
                                Console.WriteLine($"版本检验错误！");
                                return;
                            }
                        }
                        string walletOfOwner;
                        string notifyMsg;
                        {
                            {
                                var ownerArray = new byte[33];
                                dealWithData(fs, ref position, ownerArray);

                                var publicKeyOwner = GetXYByByte33(ownerArray, out isRight);
                                if (!isRight)
                                {
                                    Console.WriteLine($"解压时，解析错误！");
                                    return;
                                }
                                walletOfOwner = PublicKeyF.GetAddressOfcompressed(publicKeyOwner);
                                //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                                var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyOwner);
                                //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                                //    Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                                notifyMsg = $"输入拖入地址{walletOfUncompressed}或地址{walletOfOwner}的私钥，钥匙.txt！";
                            }
                        }


                        int fileNameLength;
                        string fileName;
                        {
                            byte[] fileNameLengthbytes = new byte[1];
                            dealWithData(fs, ref position, fileNameLengthbytes);
                            byte fileNameLengthbyte = fileNameLengthbytes[0];
                            fileNameLength = Convert.ToInt32(fileNameLengthbyte);
                            if (fileNameLength < 1)
                            {
                                Console.WriteLine($"文件解析错误！！！");
                                return;
                            }

                            var fileNameArray = new byte[fileNameLength];
                            dealWithData(fs, ref position, fileNameArray);


                            fileName = UTF8Encoding.UTF8.GetString(fileNameArray);
                            Console.WriteLine($"文件名为：{fileName}");
                        }

                        int remarkLength;
                        {
                            byte[] remarkLengthbytes = new byte[2];
                            dealWithData(fs, ref position, remarkLengthbytes);

                            byte remarkLengthbyte1 = remarkLengthbytes[0];
                            byte remarkLengthbyte2 = remarkLengthbytes[1];

                            remarkLength = Convert.ToInt32(remarkLengthbyte1) * 256 + Convert.ToInt32(remarkLengthbyte2);

                            var remarkArray = new byte[remarkLength];
                            dealWithData(fs, ref position, remarkArray);

                            Console.WriteLine($"{UTF8Encoding.UTF8.GetString(remarkArray)}");


                        }

                        System.Numerics.BigInteger privateBigInteger;
                        {
                            Console.WriteLine(notifyMsg);
                            Console.WriteLine("或直接输入私钥^[5KL][1-9A-HJ-NP-Za-km-z]{50,51}$");
                            var input = Console.ReadLine();


                            string privateKeyOfSender;
                            //^[5KL][1-9A-HJ-NP-Za-km-z]{50,51}$
                            if ((new Regex("^[5KL][1-9A-HJ-NP-Za-km-z]{50,51}$")).IsMatch(input))
                            {
                                privateKeyOfSender = input;
                            }
                            else
                            {
                                privateKeyOfSender = LockAndKeyRead.Get(input);
                            } 
                            if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                            else
                            {
                                Console.WriteLine($"请输入正确的解压私钥！！！");
                                return;
                            }
                            var publicKey2 = Calculate.getPublicByPrivate(privateBigInteger);

                            var walletOfOwner2 = PublicKeyF.GetAddressOfcompressed(publicKey2);

                            if (walletOfOwner2 == walletOfOwner) { }
                            else
                            {
                                Console.WriteLine("请输入正确的私钥");
                                return;
                            }
                        }

                        if (lengthOfFs >= position + 66 * 256 * PublicKeyComPress.HardValue)//
                        {
                            {
                                for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                                {
                                    //var s1 = ReadBytes(bytes, 33, ref position);// br.ReadBytes(33);
                                    //var s2 = ReadBytes(bytes, 33, ref position);
                                    byte[] s1 = new byte[33];
                                    dealWithData(fs, ref position, s1);

                                    byte[] s2 = new byte[33];
                                    dealWithData(fs, ref position, s2);

                                    var infomationOfSecretText = Hex.BytesToHex(s1);
                                    Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                                    bool isZero;
                                    var C1 = GetXYByByte33(s1, out isRight);
                                    if (!isRight)
                                    {
                                        Console.WriteLine($"解压时，解析错误！");
                                        return;
                                    }
                                    if (s2[0] == 0x02)
                                    {
                                        s2[0] = 0x03;
                                    }
                                    else if (s2[0] == 0x03)
                                    {
                                        s2[0] = 0x02;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"解压时，解析错误！");
                                        return;
                                    }
                                    var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                                    if (!isRight)
                                    {
                                        Console.WriteLine($"解压时，解析错误！");
                                        return;
                                    }
                                    var C3 = Calculate.getMulValue(privateBigInteger, C2_);
                                    var C6 = Calculate.pointPlus(C1, C3, out isZero);
                                    if (isZero)
                                    {
                                        Console.WriteLine($"解压时，解析错误！");
                                        return;
                                    }

                                    var M = PublicKeyComPress.ComPressPublic(C6);
                                    var key = Hex.BytesToHex(M);
                                    if (originalTextToSecretText.ContainsKey(key))
                                    {
                                        //Console.WriteLine($"解压时，包含！");
                                        originalTextToSecretText[key] = infomationOfSecretText;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"解压时，不包含！{key}");
                                        return;
                                        // return;
                                    }
                                    // materialForOrder.Add(infomation);
                                    //  materialToIndex.Add(infomation, key);

                                    // var infomation = $"{ Hex.BytesToHex(s1)}";

                                    //Console.WriteLine($"{i}压缩公钥为{infomation}");

                                    // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                                }
                                for (int i = 0; i < materialForOrder.Count; i++)
                                {
                                    materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                                }
                                //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                                // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                                //replace Byte //real To secret real Is Key
                                materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                                for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                                {
                                    byte secretByte = Convert.ToByte(mIndex % 256);
                                    materialForOrder[mIndex].secretByte = secretByte;
                                    if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                                    {
                                        Console.WriteLine("解析失败");
                                        return;
                                    }
                                    if (secretToOriginCode.ContainsKey(secretByte)) { }
                                    else
                                    {
                                        secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                                    }
                                    secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                                }

                                var fileLength =
                                    lengthOfFs
                                    - 32//lengthOfTitle
                                    - 4//lengthOfEdition
                                    - 33//sFrom.Length
                                    - 1////代表文件名字符串长度byte
                                    - fileNameLength//文件名字长度
                                    - 2////代表备注字符串长度byte
                                    - remarkLength
                                    - 66 * 256 * PublicKeyComPress.HardValue
                                    - 32
                                    - 32
                                    - 32
                                    - 33
                                    ;
                                //fileBinary = new byte[fileLength];
                                Console.WriteLine("验证完毕");

                                ToJson.Class1.Show(secretToOriginCode);
                                long sum = 0;
                                string filePathOut = "";
                                for (int i = 0; i < 1000; i++)
                                {
                                    filePathOut = $@"{fileDic}\N{i}{fileName}";
                                    if (File.Exists(filePathOut)) { }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (string.IsNullOrEmpty(filePathOut))
                                {
                                    Console.WriteLine($"解压错误");
                                    return;
                                }


                                using (FileStream fsWrite = new FileStream(filePathOut, FileMode.OpenOrCreate, System.IO.FileAccess.Write, FileShare.Write))
                                {
                                    long writePosition = 0;
                                    long addVm = 10 * 1024 * 1024;
                                    for (long i = 0; i < fileLength; i += addVm)
                                    {
                                        if (i + addVm < fileLength)
                                        {
                                            var addV = addVm;
                                            byte[] bytesToRead = new byte[addV];
                                            dealWithData(fs, ref position, bytesToRead);

                                            byte[] bytesToWrite = new byte[addV];
                                            for (var jj = 0; jj < bytesToRead.Length; jj++)
                                            {
                                                var secretByte = bytesToRead[jj];
                                                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                                var realByte = secretToOriginCode[secretByte][sheetIndex];
                                                sum += Convert.ToInt32(realByte);
                                                bytesToWrite[jj] = realByte;
                                            }
                                            fsWrite.Seek(writePosition, SeekOrigin.Begin);
                                            fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                                            writePosition += bytesToWrite.Length;
                                        }
                                        else
                                        {
                                            var addV = fileLength - i;

                                            byte[] bytesToRead = new byte[addV];
                                            dealWithData(fs, ref position, bytesToRead);

                                            byte[] bytesToWrite = new byte[addV];
                                            for (var jj = 0; jj < bytesToRead.Length; jj++)
                                            {
                                                var secretByte = bytesToRead[jj];
                                                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                                var realByte = secretToOriginCode[secretByte][sheetIndex];
                                                sum += Convert.ToInt32(realByte);
                                                bytesToWrite[jj] = realByte;
                                            }
                                            fsWrite.Seek(writePosition, SeekOrigin.Begin);
                                            fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                                            writePosition += bytesToWrite.Length;
                                        }
                                        //List<byte> bytesToWrite = new List<byte>;
                                        // dealWithData(fs, ref position)
                                        //   var secretByte = ReadByte(bytes, ref position);
                                        //  var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                        //  var realByte = secretToOriginCode[secretByte][sheetIndex];
                                        //   sum += Convert.ToInt32(realByte);
                                        //fileBinary[i] = realByte;
                                    }
                                }

                            }

                        }
                        else
                        {
                            Console.WriteLine($"解析失败！！");
                            return;
                        }
                    }

                }

                //using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                //{
                //    long length = fs.Length;

                //    // if (length >= 66 * 256 * PublicKeyComPress.HardValue)//
                //    {
                //        using (BinaryReader br = new BinaryReader(fs))
                //        {
                //            var Title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(32));
                //            if (MainParameter.Title == Title) { }
                //            for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                //            {
                //                if (i == 31)
                //                {
                //                    int a = 0;
                //                    a++;
                //                }
                //                var s1 = br.ReadBytes(33);
                //                var s2 = br.ReadBytes(33);

                //                var infomationOfSecretText = Hex.BytesToHex(s1);
                //                Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                //                bool isZero;
                //                var C1 = GetXYByByte33(s1, out isRight);
                //                if (!isRight)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                if (s2[0] == 0x02)
                //                {
                //                    s2[0] = 0x03;
                //                }
                //                else if (s2[0] == 0x03)
                //                {
                //                    s2[0] = 0x02;
                //                }
                //                else
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                //                if (!isRight)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                var C3 = Calculate.getMulValue(privateKey, C2_);
                //                var C6 = Calculate.pointPlus(C1, C3, out isZero);
                //                if (isZero)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }

                //                var M = PublicKeyComPress.ComPressPublic(C6);
                //                var key = Hex.BytesToHex(M);
                //                if (originalTextToSecretText.ContainsKey(key))
                //                {
                //                    //Console.WriteLine($"解压时，包含！");
                //                    originalTextToSecretText[key] = infomationOfSecretText;
                //                }
                //                else
                //                {
                //                    Console.WriteLine($"解压时，不包含！{key}");
                //                    return;
                //                    // return;
                //                }
                //                // materialForOrder.Add(infomation);
                //                //  materialToIndex.Add(infomation, key);

                //                // var infomation = $"{ Hex.BytesToHex(s1)}";

                //                //Console.WriteLine($"{i}压缩公钥为{infomation}");

                //                // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                //            }
                //            for (int i = 0; i < materialForOrder.Count; i++)
                //            {
                //                materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                //            }
                //            //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                //            // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                //            //replace Byte //real To secret real Is Key
                //            materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                //            for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                //            {
                //                byte secretByte = Convert.ToByte(mIndex % 256);
                //                materialForOrder[mIndex].secretByte = secretByte;
                //                if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                //                {
                //                    Console.WriteLine("解析失败");
                //                    return;
                //                }
                //                if (secretToOriginCode.ContainsKey(secretByte)) { }
                //                else
                //                {
                //                    secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                //                }
                //                secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                //            }

                //            var fileLength = length - 66 * 256 * PublicKeyComPress.HardValue;
                //            fileBinary = new byte[fileLength];
                //            Console.WriteLine("验证完毕");

                //            ToJson.Class1.Show(secretToOriginCode);
                //            long sum = 0;

                //            for (int i = 0; i < fileLength; i++)
                //            {
                //                var secretByte = br.ReadByte();
                //                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                //                var realByte = secretToOriginCode[secretByte][sheetIndex];
                //                sum += Convert.ToInt32(realByte);
                //                fileBinary[i] = realByte;
                //            }
                //            // materialForSave[position++] = relaceByte[br.ReadByte()];
                //        }


                //    }
                //    else
                //    {
                //        Console.WriteLine($"解析失败！！");
                //        return;
                //    }
                //}


            }
            {
                //string itemPath = $@"F:\工作\201909\DLQU0968N.MP4";
                //System.IO.File.WriteAllBytes(itemPath, fileBinary);
                //Console.WriteLine("文件加密成功！！！");
            }
        }

        public static void DecryptFiles()
        {
            var notifyMsg = $"输入拖入钥匙.txt！";
            string walletOfOwner2;
            System.Numerics.BigInteger privateBigInteger;
            {
                Console.WriteLine(notifyMsg);

                var privateKeyOfSender = LockAndKeyRead.Get(Console.ReadLine());

                if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                else
                {
                    Console.WriteLine($"请输入正确的解压私钥！！！");
                    return;
                }
                var publicKey2 = Calculate.getPublicByPrivate(privateBigInteger);

                walletOfOwner2 = PublicKeyF.GetAddressOfcompressed(publicKey2);

                //if (walletOfOwner2 == walletOfOwner) { }
                //else
                //{
                //    Console.WriteLine("请输入正确的私钥");
                //    return;
                //}
            }

            Console.WriteLine("输入文件夹，如F:\\生活\\ysecret");
            DirectoryInfo root = new DirectoryInfo(Console.ReadLine());
            FileInfo[] files = root.GetFiles();

            //SHA256 sha256 = new SHA256Managed();


            for (int indexOfFileForCompressing = 0; indexOfFileForCompressing < files.Length; indexOfFileForCompressing++)
            {
                bool isRight;
                Console.WriteLine($"{files[indexOfFileForCompressing].FullName}");
                //Console.WriteLine($"{}");
                if (files[indexOfFileForCompressing].Extension == ".secr")
                {
                    Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();

                    //密码→原码对照表，int，表征sheetNum
                    Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //用于排序
                    List<PublicKeyComPress.InfomationClass> materialForOrder = new List<PublicKeyComPress.InfomationClass>();
                    //
                    for (var i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                    {
                        var M = Calculate.getPublicByPrivate(i + 1);
                        var sM = PublicKeyComPress.ComPressPublic(M);
                        //bool isRight;
                        Deciphering.GetXYByByte33(sM, out isRight);
                        if (isRight) { }
                        else
                        {
                            Console.WriteLine($"sM压缩失败！");
                            return;
                        }
                        var infomationOfOriginalText = Hex.BytesToHex(sM);
                        materialForOrder.Add(new PublicKeyComPress.InfomationClass()
                        {
                            infomationOfOriginalText = infomationOfOriginalText,
                            step = i / 256,
                            responRealByte = Convert.ToByte(i % 256),
                        });
                        originalTextToSecretText.Add(infomationOfOriginalText, "");

                        //  materialForOrder.Add(key);
                        //var infomation = $"{ Hex.BytesToHex(s1)}__{ Hex.BytesToHex(s2)}";
                    }
                    Dictionary<string, string> materialToIndex = new Dictionary<string, string>();

                    SHA256 sha256 = new SHA256Managed();
                    {
                        Console.WriteLine($"输入文件名，拖入文件即可");

                        string itemPath = files[indexOfFileForCompressing].FullName; ;
                        string fileDic = System.IO.Path.GetDirectoryName(itemPath);
                        {
                            byte[] hash1, r, s, publicKeyS1;
                            long lengthOfFs;
                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                lengthOfFs = fs.Length;
                                if (lengthOfFs > 130)
                                {
                                    fs.Seek(-129, SeekOrigin.End);
                                    hash1 = new byte[32];
                                    fs.Read(hash1, 0, 32);

                                    r = new byte[32];
                                    fs.Seek(-97, SeekOrigin.End);
                                    fs.Read(r, 0, 32);

                                    s = new byte[32];
                                    fs.Seek(-65, SeekOrigin.End);
                                    fs.Read(s, 0, 32);

                                    publicKeyS1 = new byte[33];
                                    fs.Seek(-33, SeekOrigin.End);
                                    fs.Read(publicKeyS1, 0, 33);

                                    List<Byte> bytes = new List<byte>();
                                    for (var i = 0; i < 129; i++)
                                    {
                                        bytes.Add(0);
                                    }
                                    fs.Seek(-129, SeekOrigin.End);
                                    fs.Write(bytes.ToArray(), 0, 129);

                                }
                                else
                                {
                                    Console.WriteLine($"文件长度不够！！！");
                                    return;
                                }
                            }

                            byte[] hashCode;
                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {
                                hashCode = sha256.ComputeHash(fs);
                            }
                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                var length = fs.Length;
                                if (length > 130)
                                {
                                    fs.Seek(-129, SeekOrigin.End);
                                    fs.Write(hash1, 0, 32);

                                    fs.Seek(-97, SeekOrigin.End);
                                    fs.Write(r, 0, 32);

                                    fs.Seek(-65, SeekOrigin.End);
                                    fs.Write(s, 0, 32);

                                    fs.Seek(-33, SeekOrigin.End);
                                    fs.Write(publicKeyS1, 0, 33);

                                }
                                else
                                {
                                    Console.WriteLine($"文件长度不够！！！");
                                    return;
                                }
                            }
                            if (Bytes32.ByteArrayEqual(hash1, hashCode))
                            {
                                Console.WriteLine($"文件哈希验证成功");

                            }
                            else
                            {
                                Console.WriteLine($"文件哈希验证不成功,输入y/Y表示继续");
                                var y = Console.ReadLine().Trim();
                                if (y.ToLower() == "y") { }
                                else
                                {
                                    return;
                                }
                            }
                            var rNum = Bytes32.ConvetToBigInteger(r);
                            var sNum = Bytes32.ConvetToBigInteger(s);

                            var publicKeyC1 = GetXYByByte33(publicKeyS1, out isRight);
                            if (!isRight)
                            {
                                Console.WriteLine($"解压时，解析错误！");
                                return;
                            }
                            // walletOfcompressed;
                            //string notify
                            if (Sign.VerifySignature(publicKeyC1, hashCode, rNum, sNum))
                            {
                                string walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKeyC1);
                                //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                                var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyC1);
                                //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                                Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                            }
                            else
                            {
                                Console.WriteLine($"校验成功，签名不成功！输入y/Y表示继续；");
                                //return; 
                                var y = Console.ReadLine().Trim();
                                if (y.ToLower() == "y") { }
                                else
                                {
                                    return;
                                }
                            }

                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {
                                long position = 0;
                                {
                                    var titleArray = new byte[32];
                                    dealWithData(fs, ref position, titleArray);

                                    var Title = ASCIIEncoding.ASCII.GetString(titleArray);
                                    if (MainParameter.Title == Title)
                                    {
                                        Console.WriteLine(MainParameter.Title);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"文件头错误！");
                                        return;
                                    }
                                }
                                {
                                    var editionArray = new byte[4];
                                    dealWithData(fs, ref position, editionArray);
                                    byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                                    if (Bytes32.ByteArrayEqual(edition, editionArray))
                                    {
                                        Console.WriteLine("版本检验正确！！！");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"版本检验错误！");
                                        return;
                                    }
                                }
                                string walletOfOwner;
                                //string notifyMsg;
                                {
                                    {
                                        var ownerArray = new byte[33];
                                        dealWithData(fs, ref position, ownerArray);

                                        var publicKeyOwner = GetXYByByte33(ownerArray, out isRight);
                                        if (!isRight)
                                        {
                                            Console.WriteLine($"解压时，解析错误！");
                                            return;
                                        }
                                        walletOfOwner = PublicKeyF.GetAddressOfcompressed(publicKeyOwner);
                                        //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                                        var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyOwner);
                                        //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                                        //    Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                                        notifyMsg = $"输入拖入地址{walletOfUncompressed}或地址{walletOfOwner}的私钥，钥匙.txt！";
                                    }
                                }


                                int fileNameLength;
                                string fileName;
                                {
                                    byte[] fileNameLengthbytes = new byte[1];
                                    dealWithData(fs, ref position, fileNameLengthbytes);
                                    byte fileNameLengthbyte = fileNameLengthbytes[0];
                                    fileNameLength = Convert.ToInt32(fileNameLengthbyte);
                                    if (fileNameLength < 1)
                                    {
                                        Console.WriteLine($"文件解析错误！！！");
                                        return;
                                    }

                                    var fileNameArray = new byte[fileNameLength];
                                    dealWithData(fs, ref position, fileNameArray);


                                    fileName = UTF8Encoding.UTF8.GetString(fileNameArray);
                                    Console.WriteLine($"文件名为：{fileName}");
                                }

                                int remarkLength;
                                {
                                    byte[] remarkLengthbytes = new byte[2];
                                    dealWithData(fs, ref position, remarkLengthbytes);

                                    byte remarkLengthbyte1 = remarkLengthbytes[0];
                                    byte remarkLengthbyte2 = remarkLengthbytes[1];

                                    remarkLength = Convert.ToInt32(remarkLengthbyte1) * 256 + Convert.ToInt32(remarkLengthbyte2);

                                    var remarkArray = new byte[remarkLength];
                                    dealWithData(fs, ref position, remarkArray);

                                    Console.WriteLine($"{UTF8Encoding.UTF8.GetString(remarkArray)}");


                                }

                                // System.Numerics.BigInteger privateBigInteger;
                                {
                                    //Console.WriteLine(notifyMsg);

                                    //var privateKeyOfSender = LockAndKeyRead.Get(Console.ReadLine());

                                    //if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                                    //else
                                    //{
                                    //    Console.WriteLine($"请输入正确的解压私钥！！！");
                                    //    return;
                                    //}
                                    //var publicKey2 = Calculate.getPublicByPrivate(privateBigInteger);

                                    //var walletOfOwner2 = PublicKeyF.GetAddressOfcompressed(publicKey2);

                                    if (walletOfOwner2 == walletOfOwner) { }
                                    else
                                    {
                                        Console.WriteLine($"请输入{walletOfOwner}的私钥");
                                        Console.ReadLine();
                                        return;
                                    }
                                }

                                if (lengthOfFs >= position + 66 * 256 * PublicKeyComPress.HardValue)//
                                {
                                    {
                                        for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                                        {
                                            //var s1 = ReadBytes(bytes, 33, ref position);// br.ReadBytes(33);
                                            //var s2 = ReadBytes(bytes, 33, ref position);
                                            byte[] s1 = new byte[33];
                                            dealWithData(fs, ref position, s1);

                                            byte[] s2 = new byte[33];
                                            dealWithData(fs, ref position, s2);

                                            var infomationOfSecretText = Hex.BytesToHex(s1);
                                            //Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                                            bool isZero;
                                            var C1 = GetXYByByte33(s1, out isRight);
                                            if (!isRight)
                                            {
                                                Console.WriteLine($"解压时，解析错误！");
                                                return;
                                            }
                                            if (s2[0] == 0x02)
                                            {
                                                s2[0] = 0x03;
                                            }
                                            else if (s2[0] == 0x03)
                                            {
                                                s2[0] = 0x02;
                                            }
                                            else
                                            {
                                                Console.WriteLine($"解压时，解析错误！");
                                                return;
                                            }
                                            var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                                            if (!isRight)
                                            {
                                                Console.WriteLine($"解压时，解析错误！");
                                                return;
                                            }
                                            var C3 = Calculate.getMulValue(privateBigInteger, C2_);
                                            var C6 = Calculate.pointPlus(C1, C3, out isZero);
                                            if (isZero)
                                            {
                                                Console.WriteLine($"解压时，解析错误！");
                                                return;
                                            }

                                            var M = PublicKeyComPress.ComPressPublic(C6);
                                            var key = Hex.BytesToHex(M);
                                            if (originalTextToSecretText.ContainsKey(key))
                                            {
                                                //Console.WriteLine($"解压时，包含！");
                                                originalTextToSecretText[key] = infomationOfSecretText;
                                            }
                                            else
                                            {
                                                Console.WriteLine($"解压时，不包含！{key}");
                                                return;
                                                // return;
                                            }
                                            // materialForOrder.Add(infomation);
                                            //  materialToIndex.Add(infomation, key);

                                            // var infomation = $"{ Hex.BytesToHex(s1)}";

                                            //Console.WriteLine($"{i}压缩公钥为{infomation}");

                                            // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                                        }
                                        for (int i = 0; i < materialForOrder.Count; i++)
                                        {
                                            materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                                        }
                                        //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                                        // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                                        //replace Byte //real To secret real Is Key
                                        materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                                        for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                                        {
                                            byte secretByte = Convert.ToByte(mIndex % 256);
                                            materialForOrder[mIndex].secretByte = secretByte;
                                            if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                                            {
                                                Console.WriteLine("解析失败");
                                                return;
                                            }
                                            if (secretToOriginCode.ContainsKey(secretByte)) { }
                                            else
                                            {
                                                secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                                            }
                                            secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                                        }

                                        var fileLength =
                                            lengthOfFs
                                            - 32//lengthOfTitle
                                            - 4//lengthOfEdition
                                            - 33//sFrom.Length
                                            - 1////代表文件名字符串长度byte
                                            - fileNameLength//文件名字长度
                                            - 2////代表备注字符串长度byte
                                            - remarkLength
                                            - 66 * 256 * PublicKeyComPress.HardValue
                                            - 32
                                            - 32
                                            - 32
                                            - 33
                                            ;
                                        //fileBinary = new byte[fileLength];
                                        Console.WriteLine("验证完毕");

                                        ToJson.Class1.Show(secretToOriginCode);
                                        long sum = 0;
                                        string filePathOut = "";
                                        filePathOut = $@"{fileDic}\{fileName}";
                                        if (File.Exists(filePathOut))
                                        {
                                            for (int i = 0; i < 1000; i++)
                                            {
                                                filePathOut = $@"{fileDic}\N{i}{fileName}";
                                                if (File.Exists(filePathOut)) { }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        if (string.IsNullOrEmpty(filePathOut))
                                        {
                                            Console.WriteLine($"解压错误");
                                            return;
                                        }


                                        using (FileStream fsWrite = new FileStream(filePathOut, FileMode.OpenOrCreate, System.IO.FileAccess.Write, FileShare.Write))
                                        {
                                            long writePosition = 0;
                                            long addVm = 10 * 1024 * 1024;
                                            for (long i = 0; i < fileLength; i += addVm)
                                            {
                                                if (i + addVm < fileLength)
                                                {
                                                    var addV = addVm;
                                                    byte[] bytesToRead = new byte[addV];
                                                    dealWithData(fs, ref position, bytesToRead);

                                                    byte[] bytesToWrite = new byte[addV];
                                                    for (var jj = 0; jj < bytesToRead.Length; jj++)
                                                    {
                                                        var secretByte = bytesToRead[jj];
                                                        var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                                        var realByte = secretToOriginCode[secretByte][sheetIndex];
                                                        sum += Convert.ToInt32(realByte);
                                                        bytesToWrite[jj] = realByte;
                                                    }
                                                    fsWrite.Seek(writePosition, SeekOrigin.Begin);
                                                    fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                                                    writePosition += bytesToWrite.Length;
                                                }
                                                else
                                                {
                                                    var addV = fileLength - i;

                                                    byte[] bytesToRead = new byte[addV];
                                                    dealWithData(fs, ref position, bytesToRead);

                                                    byte[] bytesToWrite = new byte[addV];
                                                    for (var jj = 0; jj < bytesToRead.Length; jj++)
                                                    {
                                                        var secretByte = bytesToRead[jj];
                                                        var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                                        var realByte = secretToOriginCode[secretByte][sheetIndex];
                                                        sum += Convert.ToInt32(realByte);
                                                        bytesToWrite[jj] = realByte;
                                                    }
                                                    fsWrite.Seek(writePosition, SeekOrigin.Begin);
                                                    fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                                                    writePosition += bytesToWrite.Length;
                                                }
                                                //List<byte> bytesToWrite = new List<byte>;
                                                // dealWithData(fs, ref position)
                                                //   var secretByte = ReadByte(bytes, ref position);
                                                //  var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                                //  var realByte = secretToOriginCode[secretByte][sheetIndex];
                                                //   sum += Convert.ToInt32(realByte);
                                                //fileBinary[i] = realByte;
                                            }
                                        }

                                    }

                                }
                                else
                                {
                                    Console.WriteLine($"解析失败！！");
                                    return;
                                }
                            }

                        }

                        //using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                        //{
                        //    long length = fs.Length;

                        //    // if (length >= 66 * 256 * PublicKeyComPress.HardValue)//
                        //    {
                        //        using (BinaryReader br = new BinaryReader(fs))
                        //        {
                        //            var Title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(32));
                        //            if (MainParameter.Title == Title) { }
                        //            for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                        //            {
                        //                if (i == 31)
                        //                {
                        //                    int a = 0;
                        //                    a++;
                        //                }
                        //                var s1 = br.ReadBytes(33);
                        //                var s2 = br.ReadBytes(33);

                        //                var infomationOfSecretText = Hex.BytesToHex(s1);
                        //                Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                        //                bool isZero;
                        //                var C1 = GetXYByByte33(s1, out isRight);
                        //                if (!isRight)
                        //                {
                        //                    Console.WriteLine($"解压时，解析错误！");
                        //                    return;
                        //                }
                        //                if (s2[0] == 0x02)
                        //                {
                        //                    s2[0] = 0x03;
                        //                }
                        //                else if (s2[0] == 0x03)
                        //                {
                        //                    s2[0] = 0x02;
                        //                }
                        //                else
                        //                {
                        //                    Console.WriteLine($"解压时，解析错误！");
                        //                    return;
                        //                }
                        //                var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                        //                if (!isRight)
                        //                {
                        //                    Console.WriteLine($"解压时，解析错误！");
                        //                    return;
                        //                }
                        //                var C3 = Calculate.getMulValue(privateKey, C2_);
                        //                var C6 = Calculate.pointPlus(C1, C3, out isZero);
                        //                if (isZero)
                        //                {
                        //                    Console.WriteLine($"解压时，解析错误！");
                        //                    return;
                        //                }

                        //                var M = PublicKeyComPress.ComPressPublic(C6);
                        //                var key = Hex.BytesToHex(M);
                        //                if (originalTextToSecretText.ContainsKey(key))
                        //                {
                        //                    //Console.WriteLine($"解压时，包含！");
                        //                    originalTextToSecretText[key] = infomationOfSecretText;
                        //                }
                        //                else
                        //                {
                        //                    Console.WriteLine($"解压时，不包含！{key}");
                        //                    return;
                        //                    // return;
                        //                }
                        //                // materialForOrder.Add(infomation);
                        //                //  materialToIndex.Add(infomation, key);

                        //                // var infomation = $"{ Hex.BytesToHex(s1)}";

                        //                //Console.WriteLine($"{i}压缩公钥为{infomation}");

                        //                // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                        //            }
                        //            for (int i = 0; i < materialForOrder.Count; i++)
                        //            {
                        //                materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                        //            }
                        //            //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                        //            // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                        //            //replace Byte //real To secret real Is Key
                        //            materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                        //            for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                        //            {
                        //                byte secretByte = Convert.ToByte(mIndex % 256);
                        //                materialForOrder[mIndex].secretByte = secretByte;
                        //                if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                        //                {
                        //                    Console.WriteLine("解析失败");
                        //                    return;
                        //                }
                        //                if (secretToOriginCode.ContainsKey(secretByte)) { }
                        //                else
                        //                {
                        //                    secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                        //                }
                        //                secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                        //            }

                        //            var fileLength = length - 66 * 256 * PublicKeyComPress.HardValue;
                        //            fileBinary = new byte[fileLength];
                        //            Console.WriteLine("验证完毕");

                        //            ToJson.Class1.Show(secretToOriginCode);
                        //            long sum = 0;

                        //            for (int i = 0; i < fileLength; i++)
                        //            {
                        //                var secretByte = br.ReadByte();
                        //                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                        //                var realByte = secretToOriginCode[secretByte][sheetIndex];
                        //                sum += Convert.ToInt32(realByte);
                        //                fileBinary[i] = realByte;
                        //            }
                        //            // materialForSave[position++] = relaceByte[br.ReadByte()];
                        //        }


                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine($"解析失败！！");
                        //        return;
                        //    }
                        //}


                    }
                }
                else
                {
                    continue;
                }
            }

        }


        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {

                using (FileStream file = new FileStream(fileName, FileMode.Open))
                {
                    using (System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        byte[] retVal = md5.ComputeHash(file);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static void Decrypt2()
        {
            bool isRight;
            //Console.WriteLine($"输入私钥！");
            // var privateKey = HexToBigInteger.inputHex("e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355");

            //var inputSting = "e8d96a53e9c597e5a1e2ceaddd0b5ebe75588b26e71846b46a9b5f3666409355";
            //var inputSting = Console.ReadLine();
            //var privateKey = Hex.HexToBigInteger(inputSting);
            //privateKey = privateKey % Secp256k1.q;
            //var privateByte = Hex.HexToBytes32(inputSting);

            ////压缩公钥和原始数据的对照表
            //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();

            //Dictionary<string, byte> realInfo = new Dictionary<string, byte>();
            //
            Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();

            //密码→原码对照表，int，表征sheetNum
            Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

            //用于排序
            List<PublicKeyComPress.InfomationClass> materialForOrder = new List<PublicKeyComPress.InfomationClass>();
            //
            for (var i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
            {
                var M = Calculate.getPublicByPrivate(i + 1);
                var sM = PublicKeyComPress.ComPressPublic(M);
                //bool isRight;
                Deciphering.GetXYByByte33(sM, out isRight);
                if (isRight) { }
                else
                {
                    Console.WriteLine($"sM压缩失败！");
                    return;
                }
                var infomationOfOriginalText = Hex.BytesToHex(sM);
                materialForOrder.Add(new PublicKeyComPress.InfomationClass()
                {
                    infomationOfOriginalText = infomationOfOriginalText,
                    step = i / 256,
                    responRealByte = Convert.ToByte(i % 256),
                });
                originalTextToSecretText.Add(infomationOfOriginalText, "");

                //  materialForOrder.Add(key);
                //var infomation = $"{ Hex.BytesToHex(s1)}__{ Hex.BytesToHex(s2)}";
            }
            Dictionary<string, string> materialToIndex = new Dictionary<string, string>();

            SHA256 sha256 = new SHA256Managed();
            //bool isRight;
            byte[] fileBinary;
            byte[] bytes;

            {
                Console.WriteLine($"输入文件名，拖入文件即可");

                string itemPath = Console.ReadLine(); ;
                string fileDic = System.IO.Path.GetDirectoryName(itemPath);
                {
                    using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                    {
                        var length = fs.Length;


                        bytes = new byte[fs.Length];
                        long startCount = 0;

                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            for (long i = 0; i < length - 129; i++)
                            {
                                bytes[startCount++] = br.ReadByte();
                            }
                            var hash1 = br.ReadBytes(32);
                            var r = br.ReadBytes(32);
                            var s = br.ReadBytes(32);
                            var publicKeyS1 = br.ReadBytes(33);

                            var hashCode = sha256.ComputeHash(bytes);
                            if (Bytes32.ByteArrayEqual(hash1, hashCode))
                            {
                                Console.WriteLine($"文件哈希验证成功");

                            }
                            else
                            {
                                Console.WriteLine($"文件哈希验证不成功,输入y/Y表示继续");
                                var y = Console.ReadLine().Trim();
                                if (y.ToLower() == "y") { }
                                else
                                {
                                    return;
                                }
                            }
                            var rNum = Bytes32.ConvetToBigInteger(r);
                            var sNum = Bytes32.ConvetToBigInteger(s);

                            var publicKeyC1 = GetXYByByte33(publicKeyS1, out isRight);
                            if (!isRight)
                            {
                                Console.WriteLine($"解压时，解析错误！");
                                return;
                            }
                            // walletOfcompressed;
                            //string notify
                            if (Sign.VerifySignature(publicKeyC1, hashCode, rNum, sNum))
                            {
                                string walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKeyC1);
                                //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                                var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyC1);
                                //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                                Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                            }
                            else
                            {
                                Console.WriteLine($"校验成功，签名不成功！输入y/Y表示继续；");
                                //return; 
                                var y = Console.ReadLine().Trim();
                                if (y.ToLower() == "y") { }
                                else
                                {
                                    return;
                                }
                            }

                        }
                        ;

                    }
                }
                {
                    long position = 0;
                    {
                        var titleArray = new byte[32];
                        Array.Copy(bytes, position, titleArray, 0, 32);
                        position += 32;
                        var Title = ASCIIEncoding.ASCII.GetString(titleArray);
                        if (MainParameter.Title == Title)
                        {
                            Console.WriteLine(MainParameter.Title);
                        }
                        else
                        {
                            Console.WriteLine($"文件头错误！");
                            return;
                        }
                    }
                    {
                        var editionArray = new byte[4];
                        Array.Copy(bytes, position, editionArray, 0, 4);
                        position += 4;
                        byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                        if (Bytes32.ByteArrayEqual(edition, editionArray))
                        {
                            Console.WriteLine("版本检验正确！！！");
                        }
                        else
                        {
                            Console.WriteLine($"版本检验错误！");
                            return;
                        }
                    }
                    string walletOfOwner;
                    string notifyMsg;
                    {
                        var ownerArray = new byte[33];
                        Array.Copy(bytes, position, ownerArray, 0, 33);
                        position += 33;
                        byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };

                        var publicKeyOwner = GetXYByByte33(ownerArray, out isRight);
                        if (!isRight)
                        {
                            Console.WriteLine($"解压时，解析错误！");
                            return;
                        }

                        walletOfOwner = PublicKeyF.GetAddressOfcompressed(publicKeyOwner);
                        //Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                        var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyOwner);
                        //Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                        //    Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
                        notifyMsg = $"输入拖入地址{walletOfUncompressed}或地址{walletOfOwner}的私钥，钥匙.txt！";


                    }
                    int fileNameLength;
                    string fileName;
                    {
                        byte fileNameLengthbyte = bytes[position++];
                        fileNameLength = Convert.ToInt32(fileNameLengthbyte);
                        if (fileNameLength < 1)
                        {
                            Console.WriteLine($"文件解析错误！！！");
                            return;
                        }

                        var fileNameArray = new byte[fileNameLength];
                        Array.Copy(bytes, position, fileNameArray, 0, fileNameLength);
                        position += fileNameLength;


                        fileName = UTF8Encoding.UTF8.GetString(fileNameArray);
                        Console.WriteLine($"文件名为：{fileName}");
                    }
                    int remarkLength;
                    {
                        byte remarkLengthbyte1 = bytes[position++];
                        byte remarkLengthbyte2 = bytes[position++];

                        remarkLength = Convert.ToInt32(remarkLengthbyte1) * 256 + Convert.ToInt32(remarkLengthbyte2);

                        var remarkArray = new byte[remarkLength];
                        Array.Copy(bytes, position, remarkArray, 0, remarkLength);
                        position += remarkLength;

                        Console.WriteLine($"{UTF8Encoding.UTF8.GetString(remarkArray)}");
                    }
                    System.Numerics.BigInteger privateBigInteger;
                    {
                        Console.WriteLine(notifyMsg);

                        var privateKeyOfSender = LockAndKeyRead.Get(Console.ReadLine());

                        if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                        else
                        {
                            Console.WriteLine($"请输入正确的解压私钥！！！");
                            return;
                        }
                        var publicKey2 = Calculate.getPublicByPrivate(privateBigInteger);

                        var walletOfOwner2 = PublicKeyF.GetAddressOfcompressed(publicKey2);

                        if (walletOfOwner2 == walletOfOwner) { }
                        else
                        {
                            Console.WriteLine("请输入正确的私钥");
                            return;
                        }
                    }

                    if (bytes.Length >= position + 66 * 256 * PublicKeyComPress.HardValue)//
                    {
                        {
                            for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                            {
                                var s1 = ReadBytes(bytes, 33, ref position);// br.ReadBytes(33);
                                var s2 = ReadBytes(bytes, 33, ref position);

                                var infomationOfSecretText = Hex.BytesToHex(s1);
                                Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                                bool isZero;
                                var C1 = GetXYByByte33(s1, out isRight);
                                if (!isRight)
                                {
                                    Console.WriteLine($"解压时，解析错误！");
                                    return;
                                }
                                if (s2[0] == 0x02)
                                {
                                    s2[0] = 0x03;
                                }
                                else if (s2[0] == 0x03)
                                {
                                    s2[0] = 0x02;
                                }
                                else
                                {
                                    Console.WriteLine($"解压时，解析错误！");
                                    return;
                                }
                                var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                                if (!isRight)
                                {
                                    Console.WriteLine($"解压时，解析错误！");
                                    return;
                                }
                                var C3 = Calculate.getMulValue(privateBigInteger, C2_);
                                var C6 = Calculate.pointPlus(C1, C3, out isZero);
                                if (isZero)
                                {
                                    Console.WriteLine($"解压时，解析错误！");
                                    return;
                                }

                                var M = PublicKeyComPress.ComPressPublic(C6);
                                var key = Hex.BytesToHex(M);
                                if (originalTextToSecretText.ContainsKey(key))
                                {
                                    //Console.WriteLine($"解压时，包含！");
                                    originalTextToSecretText[key] = infomationOfSecretText;
                                }
                                else
                                {
                                    Console.WriteLine($"解压时，不包含！{key}");
                                    return;
                                    // return;
                                }
                                // materialForOrder.Add(infomation);
                                //  materialToIndex.Add(infomation, key);

                                // var infomation = $"{ Hex.BytesToHex(s1)}";

                                //Console.WriteLine($"{i}压缩公钥为{infomation}");

                                // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                            }
                            for (int i = 0; i < materialForOrder.Count; i++)
                            {
                                materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                            }
                            //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                            // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                            //replace Byte //real To secret real Is Key
                            materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                            for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                            {
                                byte secretByte = Convert.ToByte(mIndex % 256);
                                materialForOrder[mIndex].secretByte = secretByte;
                                if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                                {
                                    Console.WriteLine("解析失败");
                                    return;
                                }
                                if (secretToOriginCode.ContainsKey(secretByte)) { }
                                else
                                {
                                    secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                                }
                                secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                            }

                            var fileLength =
                                bytes.Length
                                - 32//lengthOfTitle
                                - 4//lengthOfEdition
                                - 33//sFrom.Length
                                - 1////代表文件名字符串长度byte
                                - fileNameLength//文件名字长度
                                - 2////代表备注字符串长度byte
                                - remarkLength
                                - 66 * 256 * PublicKeyComPress.HardValue
                                - 32
                                - 32
                                - 32
                                - 33
                                ;
                            fileBinary = new byte[fileLength];
                            Console.WriteLine("验证完毕");

                            ToJson.Class1.Show(secretToOriginCode);
                            long sum = 0;

                            for (int i = 0; i < fileLength; i++)
                            {
                                var secretByte = ReadByte(bytes, ref position);
                                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                                var realByte = secretToOriginCode[secretByte][sheetIndex];
                                sum += Convert.ToInt32(realByte);
                                fileBinary[i] = realByte;
                            }
                        }
                        //5HpHagT65TZzG1PH3CSu63k8DbpvD8s5ip4nEB3kEsrefaQ4Ksw
                        {
                            {
                                var filePathOut = $@"{fileDic}\{fileName}";
                                if (File.Exists(filePathOut))
                                {
                                    for (int i = 0; i < 100; i++)
                                    {
                                        filePathOut = $@"{fileDic}\N{i}{fileName}";
                                        if (File.Exists(filePathOut)) { }
                                        else
                                        {
                                            System.IO.File.WriteAllBytes(filePathOut, fileBinary);
                                            Console.WriteLine("文件解密成功！！！");
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    System.IO.File.WriteAllBytes(filePathOut, fileBinary);
                                    Console.WriteLine("文件解密成功！！！");
                                }

                            }


                        }
                    }
                    else
                    {
                        Console.WriteLine($"解析失败！！");
                        return;
                    }
                }
                //using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                //{
                //    long length = fs.Length;

                //    // if (length >= 66 * 256 * PublicKeyComPress.HardValue)//
                //    {
                //        using (BinaryReader br = new BinaryReader(fs))
                //        {
                //            var Title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(32));
                //            if (MainParameter.Title == Title) { }
                //            for (int i = 0; i < 256 * PublicKeyComPress.HardValue; i++)//
                //            {
                //                if (i == 31)
                //                {
                //                    int a = 0;
                //                    a++;
                //                }
                //                var s1 = br.ReadBytes(33);
                //                var s2 = br.ReadBytes(33);

                //                var infomationOfSecretText = Hex.BytesToHex(s1);
                //                Console.WriteLine($"{i}压缩公钥为{infomationOfSecretText}");

                //                bool isZero;
                //                var C1 = GetXYByByte33(s1, out isRight);
                //                if (!isRight)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                if (s2[0] == 0x02)
                //                {
                //                    s2[0] = 0x03;
                //                }
                //                else if (s2[0] == 0x03)
                //                {
                //                    s2[0] = 0x02;
                //                }
                //                else
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                var C2_ = GetXYByByte33(s2, out isRight);//这里表示-C2
                //                if (!isRight)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }
                //                var C3 = Calculate.getMulValue(privateKey, C2_);
                //                var C6 = Calculate.pointPlus(C1, C3, out isZero);
                //                if (isZero)
                //                {
                //                    Console.WriteLine($"解压时，解析错误！");
                //                    return;
                //                }

                //                var M = PublicKeyComPress.ComPressPublic(C6);
                //                var key = Hex.BytesToHex(M);
                //                if (originalTextToSecretText.ContainsKey(key))
                //                {
                //                    //Console.WriteLine($"解压时，包含！");
                //                    originalTextToSecretText[key] = infomationOfSecretText;
                //                }
                //                else
                //                {
                //                    Console.WriteLine($"解压时，不包含！{key}");
                //                    return;
                //                    // return;
                //                }
                //                // materialForOrder.Add(infomation);
                //                //  materialToIndex.Add(infomation, key);

                //                // var infomation = $"{ Hex.BytesToHex(s1)}";

                //                //Console.WriteLine($"{i}压缩公钥为{infomation}");

                //                // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                //            }
                //            for (int i = 0; i < materialForOrder.Count; i++)
                //            {
                //                materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                //            }
                //            //   materialForOrder = (from item in materialForOrder orderby item ascending select item).ToList();
                //            // materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                //            //replace Byte //real To secret real Is Key
                //            materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();                                                                                             //  Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                //            for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                //            {
                //                byte secretByte = Convert.ToByte(mIndex % 256);
                //                materialForOrder[mIndex].secretByte = secretByte;
                //                if (string.IsNullOrEmpty(materialForOrder[mIndex].infomationOfSecretText))
                //                {
                //                    Console.WriteLine("解析失败");
                //                    return;
                //                }
                //                if (secretToOriginCode.ContainsKey(secretByte)) { }
                //                else
                //                {
                //                    secretToOriginCode.Add(secretByte, new Dictionary<int, byte>());
                //                }
                //                secretToOriginCode[secretByte].Add(materialForOrder[mIndex].step, materialForOrder[mIndex].responRealByte);
                //            }

                //            var fileLength = length - 66 * 256 * PublicKeyComPress.HardValue;
                //            fileBinary = new byte[fileLength];
                //            Console.WriteLine("验证完毕");

                //            ToJson.Class1.Show(secretToOriginCode);
                //            long sum = 0;

                //            for (int i = 0; i < fileLength; i++)
                //            {
                //                var secretByte = br.ReadByte();
                //                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                //                var realByte = secretToOriginCode[secretByte][sheetIndex];
                //                sum += Convert.ToInt32(realByte);
                //                fileBinary[i] = realByte;
                //            }
                //            // materialForSave[position++] = relaceByte[br.ReadByte()];
                //        }


                //    }
                //    else
                //    {
                //        Console.WriteLine($"解析失败！！");
                //        return;
                //    }
                //}


            }
            {
                //string itemPath = $@"F:\工作\201909\DLQU0968N.MP4";
                //System.IO.File.WriteAllBytes(itemPath, fileBinary);
                //Console.WriteLine("文件加密成功！！！");
            }
        }

        private static byte ReadByte(byte[] bytes, ref long position)
        {
            return bytes[position++];
        }

        private static byte[] ReadBytes(byte[] bytes, int v, ref long position)
        {
            var result = new byte[v];
            Array.Copy(bytes, position, result, 0, v);
            position += v;
            return result;
        }

        public static System.Numerics.BigInteger[] GetXYByByte33(byte[] s1, out bool isRight)
        {
            var data = Hex.BytesToHex(s1);
            var pre = data.Substring(0, 2);
            data = data.Substring(2, data.Length - 2);
            // Console.WriteLine($"{data}");
            var X = HexToBigInteger.inputHex(data) % Secp256k1.p;
            var Y = Calculate.GetYByX(X);
            if (pre == "02")
            {
                if (Y.IsEven)
                { }
                else
                {
                    Y = ((Secp256k1.p - Y) + Secp256k1.p) % Secp256k1.p;
                }
            }
            else
            {
                if (Y.IsEven)
                {
                    Y = ((Secp256k1.p - Y) + Secp256k1.p) % Secp256k1.p;
                }
                else { }
            }
            //  Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
            if (Calculate.CheckXYIsRight(X, Y))
            {
                isRight = true;
                return new System.Numerics.BigInteger[] { X, Y };
            }
            else
            {
                isRight = false;
                return null;
            }
        }

        static void dealWithData(FileStream nFile, ref long position, byte[] bytesOfTitle)
        {
            nFile.Seek(position, SeekOrigin.Begin);
            nFile.Read(bytesOfTitle, 0, bytesOfTitle.Length);
            position += bytesOfTitle.Length;
        }
    }

    public class GetByName
    {
        public static void Get()
        {
            Console.WriteLine($"输入助记词！");
            string Name = Console.ReadLine();
            SHA256 sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Name + DateTime.Now.ToString("yyMMddhhssmm")));
            while (true)
            {
                //while (true)
                {
                    //Console.WriteLine($"输入助记词！");

                    hash = sha256.ComputeHash(hash);
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

                        var publicKey = Calculate.getPublicByPrivate(privateKey);
                        var walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKey, false);
                        // Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                        var kk = walletOfcompressed.Substring(1, Name.Length);
                        if (kk == Name)
                        {
                            Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                            Console.WriteLine($"您压缩后的私钥为{privateKey1}");
                            string str = System.Environment.CurrentDirectory;
                            Console.WriteLine($"程序运行目录：{str}");
                            File.WriteAllText($"{walletOfcompressed}.txt", privateKey1);
                            break;
                        }
                    }

                }
            }
        }
    }
}
