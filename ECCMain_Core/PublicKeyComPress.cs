using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class PublicKeyComPress
    {
        public class InfomationClass
        {
            public string infomationOfSecretText { get; set; }
            public string infomationOfOriginalText { get; internal set; }
            public int step { get; set; }
            public byte responRealByte { get; set; }
            public byte secretByte { get; set; }

        }
        public const int HardValue = 5;
        public delegate string inputIntoComPress();
        public delegate void outPutFromComPress(string A);
        public static void ComPress(inputIntoComPress input, outPutFromComPress output)
        {
            while (true)
            {
                output($"拖入您的Base58编码的37位或38位的私钥的路径，用此私钥进行验证.即校验.txt");

                var privateKeyOfSenderPath = input();



                var privateKeyOfSender = LockAndKeyRead.Get(privateKeyOfSenderPath);
                System.Numerics.BigInteger privateBigInteger;
                if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                else
                {
                    output($"请输入正确的私钥！！！");
                    return;
                }

                output("拖入您的输入目标的压缩公钥。即锁.txt");
                var publicKeyPath = input();
                var publicKey = LockAndKeyRead.Get(publicKeyPath);
                var pre = publicKey.Substring(0, 2);
                if (publicKey.Substring(0, 2) == "02" || publicKey.Substring(0, 2) == "03")
                {
                    //原始顺序到密文
                    //Dictionary<int, string> originToSecretText = new Dictionary<int, string>();
                    //Dictionary<string, int> secretTextToOrigin = new Dictionary<string, int>();


                    //Dictionary<string, int> secretTextToSecretCode = new Dictionary<string, int>();

                    //Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();
                    //Dictionary<string, string> secretTextToOriginalText = new Dictionary<string, string>();

                    //原码→密码对照表，int，表征sheetNum
                    Dictionary<byte, Dictionary<int, byte>> originToSecretCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //密码→原码对照表，int，表征sheetNum
                    //  Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //压缩公钥和原始数据的对照表
                    //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();



                    //  压缩公钥和原始数据的对照表
                    Dictionary<string, byte[]> refreData = new Dictionary<string, byte[]>();

                    //用于排序
                    List<InfomationClass> materialForOrder = new List<InfomationClass>();


                    //replace Byte //real To secret real Is Key
                    //Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                    //Dictionary<string, string> infomation2ToInfomation = new Dictionary<string, string>();

                    publicKey = publicKey.Substring(2, publicKey.Length - 2);
                    output($"{publicKey}");
                    var X = HexToBigInteger.inputHex(publicKey);
                    var Y = Calculate.GetYByX(X);
                    //if (Calculate.CheckXYIsRight(X, Y))
                    //{ }
                    //else
                    //{
                    //    Console.WriteLine($"请输入正确的公钥");
                    //    return;
                    //}
                    if (pre == "02")
                    {
                        if (Y.IsEven)
                        { }
                        else
                        {
                            Y = (Secp256k1.p - Y);
                        }
                    }
                    else
                    {
                        if (Y.IsEven)
                        {
                            Y = (Secp256k1.p - Y);
                        }
                        else { }
                    }
                    //   Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
                    if (Calculate.CheckXYIsRight(X, Y))
                    {
                        bool isRight;
                        var sFrom = ComPressPublic(new BigInteger[] { X, Y });
                        Deciphering.GetXYByByte33(sFrom, out isRight);
                        if (isRight) { }
                        else
                        {
                            output($"sFrom压缩失败！");
                            return;
                        }

                        //   Console.WriteLine($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验成功；");
                        SHA256 sha256 = new SHA256Managed();

                        output($"输入一个随机数");
                        var xx = input() + DateTime.Now.ToString();
                        byte[] data = Encoding.UTF8.GetBytes(xx);
                        byte[] hash1 = sha256.ComputeHash(data);
                        for (var i = 0; i < 256 * HardValue; i++) //
                        {
                            while (true)
                            {
                                var M = Calculate.getPublicByPrivate(i + 1);
                                var sM = ComPressPublic(M);
                                Deciphering.GetXYByByte33(sM, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"sM压缩失败！");
                                    return;
                                }
                                var infomationOfOriginalText = $"{ Hex.BytesToHex(sM)}";
                                System.Numerics.BigInteger r = Bytes32.ConvetToBigInteger(hash1);
                                hash1 = sha256.ComputeHash(hash1);
                                //  var 
                                var mul1 = Calculate.getMulValue(r, new System.Numerics.BigInteger[] { X, Y });
                                if (mul1 == null)
                                {
                                    continue;
                                }
                                bool isZero;
                                var c1 = Calculate.pointPlus(M, mul1, out isZero);
                                if (c1 == null)
                                {
                                    continue;
                                }
                                var c2 = Calculate.getPublicByPrivate(r);
                                if (c2 == null)
                                {
                                    continue;
                                }
                                var s1 = ComPressPublic(c1);
                                var s2 = ComPressPublic(c2);

                                //bool isRight;
                                Deciphering.GetXYByByte33(s1, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s1压缩失败！");
                                    return;
                                }
                                Deciphering.GetXYByByte33(s2, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s2压缩失败！");
                                    return;
                                }
                                var infomationOfSecretText = $"{ Hex.BytesToHex(s1)}";

                                //output($"{i}压缩公钥为{infomationOfSecretText}");

                                // msgToByte.Add(infomation, Convert.ToByte(i % 256));
                                materialForOrder.Add(new InfomationClass()
                                {
                                    infomationOfSecretText = infomationOfSecretText,
                                    infomationOfOriginalText = infomationOfOriginalText,
                                    responRealByte = Convert.ToByte(i % 256),
                                    step = i / 256,

                                });
                                //infomation2ToInfomation.Add(infomation2)

                                var combineData = combineToByte66(s1, s2);
                                refreData.Add(infomationOfSecretText, combineData);
                                break;
                            }

                        }
                        materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();
                        //  materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                        //  List<int> newOrder = new List<int>();
                        for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                        {
                            var item = materialForOrder[mIndex];
                            item.secretByte = Convert.ToByte(mIndex % 256);

                            var orginCode = item.responRealByte;

                            if (originToSecretCode.ContainsKey(item.responRealByte))
                            { }
                            else
                            {
                                originToSecretCode.Add(item.responRealByte, new Dictionary<int, byte>());
                            }
                            originToSecretCode[item.responRealByte].Add(item.step, item.secretByte);

                            //if (secretToOriginCode.ContainsKey(item.secretByte)) { }
                            //else
                            //{
                            //    secretToOriginCode.Add(item.secretByte, new Dictionary<int, byte>());
                            //}
                            //secretToOriginCode[item.secretByte].Add(item.step, item.responRealByte);
                        }
                        // byte[] materialForSave;

                        string directoryName;
                        string fileNameWithoutExtension;
                        {
                            output("输入路径！！！");
                            string itemPath = input();// $@"F:\工作\201909\DLQU0968.MP4";// Console.ReadLine();//

                            string fileName;
                            byte[] fileNameBytes;

                            {
                                directoryName = System.IO.Path.GetDirectoryName(itemPath);
                                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(itemPath).Trim();//文件名  “Default.aspx”
                                string b = System.IO.Path.GetExtension(itemPath).Trim();//扩展名 “.aspx”
                                                                                        // string mid = string.IsNullOrEmpty(b) ? "" : ".";
                                fileName = $"{fileNameWithoutExtension}{b}";
                                fileNameBytes = UTF8Encoding.UTF8.GetBytes(fileName);
                                if (fileNameBytes.Length >= 256)
                                {
                                    output("文件名(最大255字节)。过长。加密失败");
                                    return;
                                }
                            }

                            string remarks = "";
                            byte[] remarksBytes;
                            {
                                output($"请输入文件的备注(最大65535字节，2W+汉字)。输入空字符串结束！");
                                while (true)
                                {
                                    var inputSelect = input();
                                    if (string.IsNullOrEmpty(inputSelect))
                                    {
                                        output($"输入C/c(continue),继续输入。其他键退出");

                                        var select = input();
                                        if (select.ToUpper() != "C")
                                        {
                                            break;
                                        }
                                    }
                                    remarks = $"{remarks}{inputSelect}{Environment.NewLine}";
                                }
                                remarksBytes = UTF8Encoding.UTF8.GetBytes(remarks);
                                if (remarksBytes.Length >= 256 * 256)
                                {
                                    output("文件备注过程(最大65535字节)。过长。加密失败");
                                    return;
                                }
                            }

                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {



                                long fsLength = fs.Length;


                                var bytesOfTitle = ASCIIEncoding.ASCII.GetBytes(MainParameter.Title);
                                var lengthOfTitle = bytesOfTitle.Length;

                                byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                                var lengthOfEdition = edition.Length;


                                //materialForSave = new byte[
                                //    lengthOfTitle
                                //    + lengthOfEdition
                                //    + sFrom.Length
                                //    + 1  //代表文件名字符串长度byte
                                //    + fileNameBytes.Length
                                //    + 2  //代表备注字符串长度，该数字用2byte显示
                                //    + remarksBytes.Length
                                //    + 66 * 256 * HardValue
                                //    + fsLength
                                //    + 32 //后97位为0的情况下的materialForSave hash值
                                //    + 32 //r
                                //    + 32 //s
                                //    + 33 //PublicKey
                                //    ];// 
                                string outPutFilePath = $@"{directoryName}\{fileNameWithoutExtension}.secr";
                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Create))
                                {
                                    long position = 0;

                                    //{
                                    //    for (var i = 0; i < bytesOfTitle.Length; i++)
                                    //    {
                                    //        nFile.Seek(position++, SeekOrigin.Begin);
                                    //        nFile.WriteByte()
                                    //    }

                                    //    //bw.Write(bytesOfTitle, position, bytesOfTitle.Length);
                                    //}


                                    //nFile.Seek(position, See)

                                    //for (var i = 0; i < bytesOfTitle.Length; i++)
                                    {
                                        dealWithData(nFile, ref position, bytesOfTitle);
                                        dealWithData(nFile, ref position, edition);
                                        dealWithData(nFile, ref position, sFrom);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(fileNameBytes.Length) });
                                        dealWithData(nFile, ref position, fileNameBytes);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(remarksBytes.Length / 256), Convert.ToByte(remarksBytes.Length % 256) });
                                        dealWithData(nFile, ref position, remarksBytes);
                                        for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)
                                        {
                                            dealWithData(nFile, ref position, refreData[materialForOrder[indexofM].infomationOfSecretText]);
                                        }
                                    }
                                    ToJson.Class1.Show(originToSecretCode);
                                    //long position = 66 * 256;
                                    long sum = 0;
                                    using (BinaryReader br = new BinaryReader(fs))
                                    {
                                        List<byte> bytesToWrite = new List<byte>();

                                        for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
                                        {
                                            var sheetIndex = Convert.ToInt32(sum % HardValue);
                                            var byteItem = br.ReadByte();
                                            var secret = originToSecretCode[byteItem][sheetIndex];
                                            var replaceByte = secret;
                                            bytesToWrite.Add(replaceByte);
                                            sum += Convert.ToInt32(byteItem);
                                            if (bytesToWrite.Count >= 10 * 1024 * 1024)
                                            {
                                                dealWithData(nFile, ref position, bytesToWrite.ToArray());
                                                bytesToWrite = new List<byte>();
                                            }
                                        }
                                        if (bytesToWrite.Count > 0)
                                        {
                                            dealWithData(nFile, ref position, bytesToWrite.ToArray());
                                        }
                                    }
                                    List<Byte> addBlockList = new List<byte>();
                                    for (var i = 0; i < 129; i++)
                                    {
                                        addBlockList.Add(0);
                                    }
                                    //byte[] addBlock = new byte[] { 0,0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
                                    dealWithData(nFile, ref position, addBlockList.ToArray());

                                }
                                byte[] hashCode;
                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
                                {
                                    hashCode = sha256.ComputeHash(nFile);
                                }

                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
                                {
                                    long position = nFile.Length - 129;
                                    dealWithData(nFile, ref position, hashCode);

                                    //byte[] randomHash;
                                    var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
                                    var r = signal[0];
                                    var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
                                    HexToByteArray.ChangeDirection(ref rByte);

                                    var s = signal[1];
                                    var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
                                    HexToByteArray.ChangeDirection(ref sByte);


                                    dealWithData(nFile, ref position, rByte);

                                    dealWithData(nFile, ref position, sByte);

                                    var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
                                    var sSender = ComPressPublic(publicKeyOfSender);
                                    if (sSender.Length != 33)
                                    {
                                        output("文件加密不成功！！！sSender长度居然不为33");
                                        return;
                                    }

                                    dealWithData(nFile, ref position, sSender);
                                }

                            }
                        }

                        //{
                        //    string itemPath = $@"{directoryName}\{fileNameWithoutExtension}.secr";

                        //    System.IO.File.WriteAllBytes(itemPath, materialForSave);
                        //    output("文件加密成功！！！");
                        //}
                    }
                    else
                    {
                        output($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验失败；");
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public static void ComPressFiles(inputIntoComPress input, outPutFromComPress output)
        {
            output($"拖入您的Base58编码的37位或38位的私钥的路径，用此私钥进行验证.即校验.txt");

            var privateKeyOfSenderPath = input();

            var privateKeyOfSender = LockAndKeyRead.Get(privateKeyOfSenderPath);
            System.Numerics.BigInteger privateBigInteger;
            if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
            else
            {
                output($"请输入正确的私钥！！！");
                return;
            }
            output("拖入您的输入目标的压缩公钥。即锁.txt");
            var publicKeyPath = input();
            var publicKey = LockAndKeyRead.Get(publicKeyPath);
            var pre = publicKey.Substring(0, 2);

            if (publicKey.Substring(0, 2) == "02" || publicKey.Substring(0, 2) == "03")
            {
                //原始顺序到密文
                //Dictionary<int, string> originToSecretText = new Dictionary<int, string>();
                //Dictionary<string, int> secretTextToOrigin = new Dictionary<string, int>();


                //Dictionary<string, int> secretTextToSecretCode = new Dictionary<string, int>();

                //Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();
                //Dictionary<string, string> secretTextToOriginalText = new Dictionary<string, string>();

                //原码→密码对照表，int，表征sheetNum
                Dictionary<byte, Dictionary<int, byte>> originToSecretCode = new Dictionary<byte, Dictionary<int, byte>>();

                //密码→原码对照表，int，表征sheetNum
                //  Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

                //压缩公钥和原始数据的对照表
                //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();



                //  压缩公钥和原始数据的对照表
                Dictionary<string, byte[]> refreData = new Dictionary<string, byte[]>();

                //用于排序
                List<InfomationClass> materialForOrder = new List<InfomationClass>();


                //replace Byte //real To secret real Is Key
                //Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                //Dictionary<string, string> infomation2ToInfomation = new Dictionary<string, string>();
                publicKey = publicKey.Substring(2, publicKey.Length - 2);
                output($"{publicKey}");
                var X = HexToBigInteger.inputHex(publicKey);
                var Y = Calculate.GetYByX(X);
                //if (Calculate.CheckXYIsRight(X, Y))
                //{ }
                //else
                //{
                //    Console.WriteLine($"请输入正确的公钥");
                //    return;
                //}
                if (pre == "02")
                {
                    if (Y.IsEven)
                    { }
                    else
                    {
                        Y = (Secp256k1.p - Y);
                    }
                }
                else
                {
                    if (Y.IsEven)
                    {
                        Y = (Secp256k1.p - Y);
                    }
                    else { }
                }
                //   Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
                if (Calculate.CheckXYIsRight(X, Y))
                {
                    bool isRight;
                    var sFrom = ComPressPublic(new BigInteger[] { X, Y });
                    Deciphering.GetXYByByte33(sFrom, out isRight);
                    if (isRight) { }
                    else
                    {
                        output($"sFrom压缩失败！");
                        return;
                    }

                    //   Console.WriteLine($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验成功；");
                    SHA256 sha256 = new SHA256Managed();

                    output($"输入一个随机数");
                    var xx = input() + DateTime.Now.ToString();
                    byte[] data = Encoding.UTF8.GetBytes(xx);
                    byte[] hash1 = sha256.ComputeHash(data);
                    for (var i = 0; i < 256 * HardValue; i++) //
                    {
                        while (true)
                        {
                            var M = Calculate.getPublicByPrivate(i + 1);
                            var sM = ComPressPublic(M);
                            Deciphering.GetXYByByte33(sM, out isRight);
                            if (isRight) { }
                            else
                            {
                                output($"sM压缩失败！");
                                return;
                            }
                            var infomationOfOriginalText = $"{ Hex.BytesToHex(sM)}";
                            System.Numerics.BigInteger r = Bytes32.ConvetToBigInteger(hash1);
                            hash1 = sha256.ComputeHash(hash1);
                            //  var 
                            var mul1 = Calculate.getMulValue(r, new System.Numerics.BigInteger[] { X, Y });
                            if (mul1 == null)
                            {
                                continue;
                            }
                            bool isZero;
                            var c1 = Calculate.pointPlus(M, mul1, out isZero);
                            if (c1 == null)
                            {
                                continue;
                            }
                            var c2 = Calculate.getPublicByPrivate(r);
                            if (c2 == null)
                            {
                                continue;
                            }
                            var s1 = ComPressPublic(c1);
                            var s2 = ComPressPublic(c2);

                            //bool isRight;
                            Deciphering.GetXYByByte33(s1, out isRight);
                            if (isRight) { }
                            else
                            {
                                output($"s1压缩失败！");
                                return;
                            }
                            Deciphering.GetXYByByte33(s2, out isRight);
                            if (isRight) { }
                            else
                            {
                                output($"s2压缩失败！");
                                return;
                            }
                            var infomationOfSecretText = $"{ Hex.BytesToHex(s1)}";

                            //output($"{i}压缩公钥为{infomationOfSecretText}");

                            // msgToByte.Add(infomation, Convert.ToByte(i % 256));
                            materialForOrder.Add(new InfomationClass()
                            {
                                infomationOfSecretText = infomationOfSecretText,
                                infomationOfOriginalText = infomationOfOriginalText,
                                responRealByte = Convert.ToByte(i % 256),
                                step = i / 256,

                            });
                            //infomation2ToInfomation.Add(infomation2)

                            var combineData = combineToByte66(s1, s2);
                            refreData.Add(infomationOfSecretText, combineData);
                            break;
                        }

                    }
                    materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();
                    //  materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                    //  List<int> newOrder = new List<int>();
                    for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                    {
                        var item = materialForOrder[mIndex];
                        item.secretByte = Convert.ToByte(mIndex % 256);

                        var orginCode = item.responRealByte;

                        if (originToSecretCode.ContainsKey(item.responRealByte))
                        { }
                        else
                        {
                            originToSecretCode.Add(item.responRealByte, new Dictionary<int, byte>());
                        }
                        originToSecretCode[item.responRealByte].Add(item.step, item.secretByte);

                        //if (secretToOriginCode.ContainsKey(item.secretByte)) { }
                        //else
                        //{
                        //    secretToOriginCode.Add(item.secretByte, new Dictionary<int, byte>());
                        //}
                        //secretToOriginCode[item.secretByte].Add(item.step, item.responRealByte);
                    }
                    // byte[] materialForSave;

                    string directoryName;
                    string fileNameWithoutExtension;
                    {
                        Console.WriteLine("输入文件夹，如F:\\生活\\ysecret");
                        DirectoryInfo root = new DirectoryInfo(Console.ReadLine());
                        FileInfo[] files = root.GetFiles();

                        for (int indexOfFileForCompressing = 0; indexOfFileForCompressing < files.Length; indexOfFileForCompressing++)
                        {
                            Console.WriteLine($"{files[indexOfFileForCompressing].FullName}");
                            //Console.WriteLine($"{}");
                            if (files[indexOfFileForCompressing].Extension == ".secr")
                            {
                                continue;
                            }

                            string itemPath = files[indexOfFileForCompressing].FullName;
                            var md5 = GetMD5HashFromFile(files[indexOfFileForCompressing].FullName);
                            directoryName = System.IO.Path.GetDirectoryName(itemPath);
                            string outPutFilePath = $@"{directoryName}\{md5}.secr";
                            if (File.Exists(outPutFilePath))
                            {
                                continue;
                            }
                            //output("输入路径！！！");
                            //if (Console.ReadLine().Trim().ToUpper() == "N")
                            //{
                            //    continue;
                            //}

                            // $@"F:\工作\201909\DLQU0968.MP4";// Console.ReadLine();//

                            string fileName;
                            byte[] fileNameBytes;

                            {
                                directoryName = System.IO.Path.GetDirectoryName(itemPath);
                                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(itemPath).Trim();//文件名  “Default.aspx”
                                string b = System.IO.Path.GetExtension(itemPath).Trim();//扩展名 “.aspx”
                                                                                        // string mid = string.IsNullOrEmpty(b) ? "" : ".";
                                fileName = $"{fileNameWithoutExtension}{b}";
                                fileNameBytes = UTF8Encoding.UTF8.GetBytes(fileName);
                                if (fileNameBytes.Length >= 256)
                                {
                                    output("文件名(最大255字节)。过长。加密失败");
                                    return;
                                }
                            }

                            string remarks = "";
                            byte[] remarksBytes;
                            {
                                output($"请输入文件的备注(最大65535字节，2W+汉字)。输入空字符串结束！");
                                while (true)
                                {
                                    var inputSelect = "ccc";
                                    //if (string.IsNullOrEmpty(inputSelect))
                                    //{
                                    //    output($"输入C/c(continue),继续输入。其他键退出");

                                    //    var select = input();
                                    //    if (select.ToUpper() != "C")
                                    //    {
                                    //        break;
                                    //    }
                                    //}
                                    remarks = $"{remarks}{inputSelect}{Environment.NewLine}";
                                    break;
                                }
                                remarksBytes = UTF8Encoding.UTF8.GetBytes(remarks);
                                if (remarksBytes.Length >= 256 * 256)
                                {
                                    output("文件备注过程(最大65535字节)。过长。加密失败");
                                    return;
                                }
                            }

                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {



                                long fsLength = fs.Length;


                                var bytesOfTitle = ASCIIEncoding.ASCII.GetBytes(MainParameter.Title);
                                var lengthOfTitle = bytesOfTitle.Length;

                                byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                                var lengthOfEdition = edition.Length;


                                //materialForSave = new byte[
                                //    lengthOfTitle
                                //    + lengthOfEdition
                                //    + sFrom.Length
                                //    + 1  //代表文件名字符串长度byte
                                //    + fileNameBytes.Length
                                //    + 2  //代表备注字符串长度，该数字用2byte显示
                                //    + remarksBytes.Length
                                //    + 66 * 256 * HardValue
                                //    + fsLength
                                //    + 32 //后97位为0的情况下的materialForSave hash值
                                //    + 32 //r
                                //    + 32 //s
                                //    + 33 //PublicKey
                                //    ];// 
                                // string outPutFilePath = $@"{directoryName}\{fileNameWithoutExtension}.secr";
                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Create))
                                {
                                    long position = 0;

                                    //{
                                    //    for (var i = 0; i < bytesOfTitle.Length; i++)
                                    //    {
                                    //        nFile.Seek(position++, SeekOrigin.Begin);
                                    //        nFile.WriteByte()
                                    //    }

                                    //    //bw.Write(bytesOfTitle, position, bytesOfTitle.Length);
                                    //}


                                    //nFile.Seek(position, See)

                                    //for (var i = 0; i < bytesOfTitle.Length; i++)
                                    {
                                        dealWithData(nFile, ref position, bytesOfTitle);
                                        dealWithData(nFile, ref position, edition);
                                        dealWithData(nFile, ref position, sFrom);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(fileNameBytes.Length) });
                                        dealWithData(nFile, ref position, fileNameBytes);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(remarksBytes.Length / 256), Convert.ToByte(remarksBytes.Length % 256) });
                                        dealWithData(nFile, ref position, remarksBytes);
                                        for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)
                                        {
                                            dealWithData(nFile, ref position, refreData[materialForOrder[indexofM].infomationOfSecretText]);
                                        }
                                    }
                                    ToJson.Class1.Show(originToSecretCode);
                                    //long position = 66 * 256;
                                    long sum = 0;
                                    using (BinaryReader br = new BinaryReader(fs))
                                    {
                                        List<byte> bytesToWrite = new List<byte>();

                                        for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
                                        {
                                            var sheetIndex = Convert.ToInt32(sum % HardValue);
                                            var byteItem = br.ReadByte();
                                            var secret = originToSecretCode[byteItem][sheetIndex];
                                            var replaceByte = secret;
                                            bytesToWrite.Add(replaceByte);
                                            sum += Convert.ToInt32(byteItem);
                                            if (bytesToWrite.Count >= 10 * 1024 * 1024)
                                            {
                                                dealWithData(nFile, ref position, bytesToWrite.ToArray());
                                                bytesToWrite = new List<byte>();
                                            }
                                        }
                                        if (bytesToWrite.Count > 0)
                                        {
                                            dealWithData(nFile, ref position, bytesToWrite.ToArray());
                                        }
                                    }
                                    List<Byte> addBlockList = new List<byte>();
                                    for (var i = 0; i < 129; i++)
                                    {
                                        addBlockList.Add(0);
                                    }
                                    //byte[] addBlock = new byte[] { 0,0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
                                    dealWithData(nFile, ref position, addBlockList.ToArray());

                                }
                                byte[] hashCode;
                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
                                {
                                    hashCode = sha256.ComputeHash(nFile);
                                }

                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
                                {
                                    long position = nFile.Length - 129;
                                    dealWithData(nFile, ref position, hashCode);

                                    //byte[] randomHash;
                                    var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
                                    var r = signal[0];
                                    var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
                                    HexToByteArray.ChangeDirection(ref rByte);

                                    var s = signal[1];
                                    var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
                                    HexToByteArray.ChangeDirection(ref sByte);


                                    dealWithData(nFile, ref position, rByte);

                                    dealWithData(nFile, ref position, sByte);

                                    var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
                                    var sSender = ComPressPublic(publicKeyOfSender);
                                    if (sSender.Length != 33)
                                    {
                                        output("文件加密不成功！！！sSender长度居然不为33");
                                        return;
                                    }

                                    dealWithData(nFile, ref position, sSender);
                                }

                            }

                        }

                    }

                    //{
                    //    string itemPath = $@"{directoryName}\{fileNameWithoutExtension}.secr";

                    //    System.IO.File.WriteAllBytes(itemPath, materialForSave);
                    //    output("文件加密成功！！！");
                    //}
                }
                else
                {
                    output($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验失败；");
                }
            }


            //while (true)
            //{







            //   
            //    {
            //        //原始顺序到密文
            //        //Dictionary<int, string> originToSecretText = new Dictionary<int, string>();
            //        //Dictionary<string, int> secretTextToOrigin = new Dictionary<string, int>();


            //        //Dictionary<string, int> secretTextToSecretCode = new Dictionary<string, int>();

            //        //Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();
            //        //Dictionary<string, string> secretTextToOriginalText = new Dictionary<string, string>();

            //        //原码→密码对照表，int，表征sheetNum
            //        Dictionary<byte, Dictionary<int, byte>> originToSecretCode = new Dictionary<byte, Dictionary<int, byte>>();

            //        //密码→原码对照表，int，表征sheetNum
            //        //  Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

            //        //压缩公钥和原始数据的对照表
            //        //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();



            //        //  压缩公钥和原始数据的对照表
            //        Dictionary<string, byte[]> refreData = new Dictionary<string, byte[]>();

            //        //用于排序
            //        List<InfomationClass> materialForOrder = new List<InfomationClass>();


            //        //replace Byte //real To secret real Is Key
            //        //Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

            //        //Dictionary<string, string> infomation2ToInfomation = new Dictionary<string, string>();

            //        publicKey = publicKey.Substring(2, publicKey.Length - 2);
            //        output($"{publicKey}");
            //        var X = HexToBigInteger.inputHex(publicKey);
            //        var Y = Calculate.GetYByX(X);
            //        //if (Calculate.CheckXYIsRight(X, Y))
            //        //{ }
            //        //else
            //        //{
            //        //    Console.WriteLine($"请输入正确的公钥");
            //        //    return;
            //        //}
            //        if (pre == "02")
            //        {
            //            if (Y.IsEven)
            //            { }
            //            else
            //            {
            //                Y = (Secp256k1.p - Y);
            //            }
            //        }
            //        else
            //        {
            //            if (Y.IsEven)
            //            {
            //                Y = (Secp256k1.p - Y);
            //            }
            //            else { }
            //        }
            //        //   Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
            //        if (Calculate.CheckXYIsRight(X, Y))
            //        {
            //            bool isRight;
            //            var sFrom = ComPressPublic(new BigInteger[] { X, Y });
            //            Deciphering.GetXYByByte33(sFrom, out isRight);
            //            if (isRight) { }
            //            else
            //            {
            //                output($"sFrom压缩失败！");
            //                return;
            //            }

            //            //   Console.WriteLine($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验成功；");
            //            SHA256 sha256 = new SHA256Managed();

            //            output($"输入一个随机数");
            //            var xx = input() + DateTime.Now.ToString();
            //            byte[] data = Encoding.UTF8.GetBytes(xx);
            //            byte[] hash1 = sha256.ComputeHash(data);
            //            for (var i = 0; i < 256 * HardValue; i++) //
            //            {
            //                while (true)
            //                {
            //                    var M = Calculate.getPublicByPrivate(i + 1);
            //                    var sM = ComPressPublic(M);
            //                    Deciphering.GetXYByByte33(sM, out isRight);
            //                    if (isRight) { }
            //                    else
            //                    {
            //                        output($"sM压缩失败！");
            //                        return;
            //                    }
            //                    var infomationOfOriginalText = $"{ Hex.BytesToHex(sM)}";
            //                    System.Numerics.BigInteger r = Bytes32.ConvetToBigInteger(hash1);
            //                    hash1 = sha256.ComputeHash(hash1);
            //                    //  var 
            //                    var mul1 = Calculate.getMulValue(r, new System.Numerics.BigInteger[] { X, Y });
            //                    if (mul1 == null)
            //                    {
            //                        continue;
            //                    }
            //                    bool isZero;
            //                    var c1 = Calculate.pointPlus(M, mul1, out isZero);
            //                    if (c1 == null)
            //                    {
            //                        continue;
            //                    }
            //                    var c2 = Calculate.getPublicByPrivate(r);
            //                    if (c2 == null)
            //                    {
            //                        continue;
            //                    }
            //                    var s1 = ComPressPublic(c1);
            //                    var s2 = ComPressPublic(c2);

            //                    //bool isRight;
            //                    Deciphering.GetXYByByte33(s1, out isRight);
            //                    if (isRight) { }
            //                    else
            //                    {
            //                        output($"s1压缩失败！");
            //                        return;
            //                    }
            //                    Deciphering.GetXYByByte33(s2, out isRight);
            //                    if (isRight) { }
            //                    else
            //                    {
            //                        output($"s2压缩失败！");
            //                        return;
            //                    }
            //                    var infomationOfSecretText = $"{ Hex.BytesToHex(s1)}";

            //                    //output($"{i}压缩公钥为{infomationOfSecretText}");

            //                    // msgToByte.Add(infomation, Convert.ToByte(i % 256));
            //                    materialForOrder.Add(new InfomationClass()
            //                    {
            //                        infomationOfSecretText = infomationOfSecretText,
            //                        infomationOfOriginalText = infomationOfOriginalText,
            //                        responRealByte = Convert.ToByte(i % 256),
            //                        step = i / 256,

            //                    });
            //                    //infomation2ToInfomation.Add(infomation2)

            //                    var combineData = combineToByte66(s1, s2);
            //                    refreData.Add(infomationOfSecretText, combineData);
            //                    break;
            //                }

            //            }
            //            materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();
            //            //  materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
            //            //  List<int> newOrder = new List<int>();
            //            for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
            //            {
            //                var item = materialForOrder[mIndex];
            //                item.secretByte = Convert.ToByte(mIndex % 256);

            //                var orginCode = item.responRealByte;

            //                if (originToSecretCode.ContainsKey(item.responRealByte))
            //                { }
            //                else
            //                {
            //                    originToSecretCode.Add(item.responRealByte, new Dictionary<int, byte>());
            //                }
            //                originToSecretCode[item.responRealByte].Add(item.step, item.secretByte);

            //                //if (secretToOriginCode.ContainsKey(item.secretByte)) { }
            //                //else
            //                //{
            //                //    secretToOriginCode.Add(item.secretByte, new Dictionary<int, byte>());
            //                //}
            //                //secretToOriginCode[item.secretByte].Add(item.step, item.responRealByte);
            //            }
            //            // byte[] materialForSave;

            //            string directoryName;
            //            string fileNameWithoutExtension;
            //            {
            //                output("输入路径！！！");
            //                string itemPath = input();// $@"F:\工作\201909\DLQU0968.MP4";// Console.ReadLine();//

            //                string fileName;
            //                byte[] fileNameBytes;

            //                {
            //                    directoryName = System.IO.Path.GetDirectoryName(itemPath);
            //                    fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(itemPath).Trim();//文件名  “Default.aspx”
            //                    string b = System.IO.Path.GetExtension(itemPath).Trim();//扩展名 “.aspx”
            //                                                                            // string mid = string.IsNullOrEmpty(b) ? "" : ".";
            //                    fileName = $"{fileNameWithoutExtension}{b}";
            //                    fileNameBytes = UTF8Encoding.UTF8.GetBytes(fileName);
            //                    if (fileNameBytes.Length >= 256)
            //                    {
            //                        output("文件名(最大255字节)。过长。加密失败");
            //                        return;
            //                    }
            //                }

            //                string remarks = "";
            //                byte[] remarksBytes;
            //                {
            //                    output($"请输入文件的备注(最大65535字节，2W+汉字)。输入空字符串结束！");
            //                    while (true)
            //                    {
            //                        var inputSelect = input();
            //                        if (string.IsNullOrEmpty(inputSelect))
            //                        {
            //                            output($"输入C/c(continue),继续输入。其他键退出");

            //                            var select = input();
            //                            if (select.ToUpper() != "C")
            //                            {
            //                                break;
            //                            }
            //                        }
            //                        remarks = $"{remarks}{inputSelect}{Environment.NewLine}";
            //                    }
            //                    remarksBytes = UTF8Encoding.UTF8.GetBytes(remarks);
            //                    if (remarksBytes.Length >= 256 * 256)
            //                    {
            //                        output("文件备注过程(最大65535字节)。过长。加密失败");
            //                        return;
            //                    }
            //                }

            //                using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
            //                {



            //                    long fsLength = fs.Length;


            //                    var bytesOfTitle = ASCIIEncoding.ASCII.GetBytes(MainParameter.Title);
            //                    var lengthOfTitle = bytesOfTitle.Length;

            //                    byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
            //                    var lengthOfEdition = edition.Length;


            //                    //materialForSave = new byte[
            //                    //    lengthOfTitle
            //                    //    + lengthOfEdition
            //                    //    + sFrom.Length
            //                    //    + 1  //代表文件名字符串长度byte
            //                    //    + fileNameBytes.Length
            //                    //    + 2  //代表备注字符串长度，该数字用2byte显示
            //                    //    + remarksBytes.Length
            //                    //    + 66 * 256 * HardValue
            //                    //    + fsLength
            //                    //    + 32 //后97位为0的情况下的materialForSave hash值
            //                    //    + 32 //r
            //                    //    + 32 //s
            //                    //    + 33 //PublicKey
            //                    //    ];// 
            //                    string outPutFilePath = $@"{directoryName}\{fileNameWithoutExtension}.secr";
            //                    using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Create))
            //                    {
            //                        long position = 0;

            //                        //{
            //                        //    for (var i = 0; i < bytesOfTitle.Length; i++)
            //                        //    {
            //                        //        nFile.Seek(position++, SeekOrigin.Begin);
            //                        //        nFile.WriteByte()
            //                        //    }

            //                        //    //bw.Write(bytesOfTitle, position, bytesOfTitle.Length);
            //                        //}


            //                        //nFile.Seek(position, See)

            //                        //for (var i = 0; i < bytesOfTitle.Length; i++)
            //                        {
            //                            dealWithData(nFile, ref position, bytesOfTitle);
            //                            dealWithData(nFile, ref position, edition);
            //                            dealWithData(nFile, ref position, sFrom);
            //                            dealWithData(nFile, ref position, new byte[] { Convert.ToByte(fileNameBytes.Length) });
            //                            dealWithData(nFile, ref position, fileNameBytes);
            //                            dealWithData(nFile, ref position, new byte[] { Convert.ToByte(remarksBytes.Length / 256), Convert.ToByte(remarksBytes.Length % 256) });
            //                            dealWithData(nFile, ref position, remarksBytes);
            //                            for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)
            //                            {
            //                                dealWithData(nFile, ref position, refreData[materialForOrder[indexofM].infomationOfSecretText]);
            //                            }
            //                        }
            //                        ToJson.Class1.Show(originToSecretCode);
            //                        //long position = 66 * 256;
            //                        long sum = 0;
            //                        using (BinaryReader br = new BinaryReader(fs))
            //                        {
            //                            List<byte> bytesToWrite = new List<byte>();

            //                            for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
            //                            {
            //                                var sheetIndex = Convert.ToInt32(sum % HardValue);
            //                                var byteItem = br.ReadByte();
            //                                var secret = originToSecretCode[byteItem][sheetIndex];
            //                                var replaceByte = secret;
            //                                bytesToWrite.Add(replaceByte);
            //                                sum += Convert.ToInt32(byteItem);
            //                                if (bytesToWrite.Count >= 10 * 1024 * 1024)
            //                                {
            //                                    dealWithData(nFile, ref position, bytesToWrite.ToArray());
            //                                    bytesToWrite = new List<byte>();
            //                                }
            //                            }
            //                            if (bytesToWrite.Count > 0)
            //                            {
            //                                dealWithData(nFile, ref position, bytesToWrite.ToArray());
            //                            }
            //                        }
            //                        List<Byte> addBlockList = new List<byte>();
            //                        for (var i = 0; i < 129; i++)
            //                        {
            //                            addBlockList.Add(0);
            //                        }
            //                        //byte[] addBlock = new byte[] { 0,0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
            //                        dealWithData(nFile, ref position, addBlockList.ToArray());

            //                    }
            //                    byte[] hashCode;
            //                    using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
            //                    {
            //                        hashCode = sha256.ComputeHash(nFile);
            //                    }

            //                    using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Open))
            //                    {
            //                        long position = nFile.Length - 129;
            //                        dealWithData(nFile, ref position, hashCode);

            //                        //byte[] randomHash;
            //                        var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
            //                        var r = signal[0];
            //                        var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
            //                        HexToByteArray.ChangeDirection(ref rByte);

            //                        var s = signal[1];
            //                        var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
            //                        HexToByteArray.ChangeDirection(ref sByte);


            //                        dealWithData(nFile, ref position, rByte);

            //                        dealWithData(nFile, ref position, sByte);

            //                        var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
            //                        var sSender = ComPressPublic(publicKeyOfSender);
            //                        if (sSender.Length != 33)
            //                        {
            //                            output("文件加密不成功！！！sSender长度居然不为33");
            //                            return;
            //                        }

            //                        dealWithData(nFile, ref position, sSender);
            //                    }

            //                }
            //            }

            //            //{
            //            //    string itemPath = $@"{directoryName}\{fileNameWithoutExtension}.secr";

            //            //    System.IO.File.WriteAllBytes(itemPath, materialForSave);
            //            //    output("文件加密成功！！！");
            //            //}
            //        }
            //        else
            //        {
            //            output($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验失败；");
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
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

        public static void ComPress2(inputIntoComPress input, outPutFromComPress output)
        {
            while (true)
            {
                output($"拖入您的Base58编码的37位或38位的私钥的路径，用此私钥进行验证.即校验.txt");

                var privateKeyOfSenderPath = input();



                var privateKeyOfSender = LockAndKeyRead.Get(privateKeyOfSenderPath);
                System.Numerics.BigInteger privateBigInteger;
                if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                else
                {
                    output($"请输入正确的私钥！！！");
                    return;
                }

                output("拖入您的输入目标的压缩公钥。即锁.txt");
                var publicKeyPath = input();
                var publicKey = LockAndKeyRead.Get(publicKeyPath);
                var pre = publicKey.Substring(0, 2);
                if (publicKey.Substring(0, 2) == "02" || publicKey.Substring(0, 2) == "03")
                {
                    //原始顺序到密文
                    //Dictionary<int, string> originToSecretText = new Dictionary<int, string>();
                    //Dictionary<string, int> secretTextToOrigin = new Dictionary<string, int>();


                    //Dictionary<string, int> secretTextToSecretCode = new Dictionary<string, int>();

                    //Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();
                    //Dictionary<string, string> secretTextToOriginalText = new Dictionary<string, string>();

                    //原码→密码对照表，int，表征sheetNum
                    Dictionary<byte, Dictionary<int, byte>> originToSecretCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //密码→原码对照表，int，表征sheetNum
                    //  Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //压缩公钥和原始数据的对照表
                    //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();



                    //  压缩公钥和原始数据的对照表
                    Dictionary<string, byte[]> refreData = new Dictionary<string, byte[]>();

                    //用于排序
                    List<InfomationClass> materialForOrder = new List<InfomationClass>();


                    //replace Byte //real To secret real Is Key
                    //Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                    //Dictionary<string, string> infomation2ToInfomation = new Dictionary<string, string>();

                    publicKey = publicKey.Substring(2, publicKey.Length - 2);
                    output($"{publicKey}");
                    var X = HexToBigInteger.inputHex(publicKey);
                    var Y = Calculate.GetYByX(X);
                    //if (Calculate.CheckXYIsRight(X, Y))
                    //{ }
                    //else
                    //{
                    //    Console.WriteLine($"请输入正确的公钥");
                    //    return;
                    //}
                    if (pre == "02")
                    {
                        if (Y.IsEven)
                        { }
                        else
                        {
                            Y = (Secp256k1.p - Y);
                        }
                    }
                    else
                    {
                        if (Y.IsEven)
                        {
                            Y = (Secp256k1.p - Y);
                        }
                        else { }
                    }
                    //   Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
                    if (Calculate.CheckXYIsRight(X, Y))
                    {
                        bool isRight;
                        var sFrom = ComPressPublic(new BigInteger[] { X, Y });
                        Deciphering.GetXYByByte33(sFrom, out isRight);
                        if (isRight) { }
                        else
                        {
                            output($"sFrom压缩失败！");
                            return;
                        }

                        //   Console.WriteLine($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验成功；");
                        SHA256 sha256 = new SHA256Managed();

                        output($"输入一个随机数");
                        var xx = input() + DateTime.Now.ToString();
                        byte[] data = Encoding.UTF8.GetBytes(xx);
                        byte[] hash1 = sha256.ComputeHash(data);
                        for (var i = 0; i < 256 * HardValue; i++) //
                        {
                            while (true)
                            {
                                var M = Calculate.getPublicByPrivate(i + 1);
                                var sM = ComPressPublic(M);
                                Deciphering.GetXYByByte33(sM, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"sM压缩失败！");
                                    return;
                                }
                                var infomationOfOriginalText = $"{ Hex.BytesToHex(sM)}";
                                System.Numerics.BigInteger r = Bytes32.ConvetToBigInteger(hash1);
                                hash1 = sha256.ComputeHash(hash1);
                                //  var 
                                var mul1 = Calculate.getMulValue(r, new System.Numerics.BigInteger[] { X, Y });
                                if (mul1 == null)
                                {
                                    continue;
                                }
                                bool isZero;
                                var c1 = Calculate.pointPlus(M, mul1, out isZero);
                                if (c1 == null)
                                {
                                    continue;
                                }
                                var c2 = Calculate.getPublicByPrivate(r);
                                if (c2 == null)
                                {
                                    continue;
                                }
                                var s1 = ComPressPublic(c1);
                                var s2 = ComPressPublic(c2);

                                //bool isRight;
                                Deciphering.GetXYByByte33(s1, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s1压缩失败！");
                                    return;
                                }
                                Deciphering.GetXYByByte33(s2, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s2压缩失败！");
                                    return;
                                }
                                var infomationOfSecretText = $"{ Hex.BytesToHex(s1)}";

                                //output($"{i}压缩公钥为{infomationOfSecretText}");

                                // msgToByte.Add(infomation, Convert.ToByte(i % 256));
                                materialForOrder.Add(new InfomationClass()
                                {
                                    infomationOfSecretText = infomationOfSecretText,
                                    infomationOfOriginalText = infomationOfOriginalText,
                                    responRealByte = Convert.ToByte(i % 256),
                                    step = i / 256,

                                });
                                //infomation2ToInfomation.Add(infomation2)

                                var combineData = combineToByte66(s1, s2);
                                refreData.Add(infomationOfSecretText, combineData);
                                break;
                            }

                        }
                        materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();
                        //  materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                        //  List<int> newOrder = new List<int>();
                        for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                        {
                            var item = materialForOrder[mIndex];
                            item.secretByte = Convert.ToByte(mIndex % 256);

                            var orginCode = item.responRealByte;

                            if (originToSecretCode.ContainsKey(item.responRealByte))
                            { }
                            else
                            {
                                originToSecretCode.Add(item.responRealByte, new Dictionary<int, byte>());
                            }
                            originToSecretCode[item.responRealByte].Add(item.step, item.secretByte);

                            //if (secretToOriginCode.ContainsKey(item.secretByte)) { }
                            //else
                            //{
                            //    secretToOriginCode.Add(item.secretByte, new Dictionary<int, byte>());
                            //}
                            //secretToOriginCode[item.secretByte].Add(item.step, item.responRealByte);
                        }
                        byte[] materialForSave;

                        string directoryName;
                        string fileNameWithoutExtension;
                        {
                            output("输入路径！！！");
                            string itemPath = input();// $@"F:\工作\201909\DLQU0968.MP4";// Console.ReadLine();//

                            string fileName;
                            byte[] fileNameBytes;

                            {
                                directoryName = System.IO.Path.GetDirectoryName(itemPath);
                                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(itemPath).Trim();//文件名  “Default.aspx”
                                string b = System.IO.Path.GetExtension(itemPath).Trim();//扩展名 “.aspx”
                                                                                        // string mid = string.IsNullOrEmpty(b) ? "" : ".";
                                fileName = $"{fileNameWithoutExtension}{b}";
                                fileNameBytes = UTF8Encoding.UTF8.GetBytes(fileName);
                                if (fileNameBytes.Length >= 256)
                                {
                                    output("文件名(最大255字节)。过长。加密失败");
                                    return;
                                }
                            }

                            string remarks = "";
                            byte[] remarksBytes;
                            {
                                output($"请输入文件的备注(最大65535字节，2W+汉字)。输入空字符串结束！");
                                while (true)
                                {
                                    var inputSelect = input();
                                    if (string.IsNullOrEmpty(inputSelect))
                                    {
                                        output($"输入C/c(continue),继续输入。其他键退出");

                                        var select = input();
                                        if (select.ToUpper() != "C")
                                        {
                                            break;
                                        }
                                    }
                                    remarks = $"{remarks}{inputSelect}{Environment.NewLine}";
                                }
                                remarksBytes = UTF8Encoding.UTF8.GetBytes(remarks);
                                if (remarksBytes.Length >= 256 * 256)
                                {
                                    output("文件备注过程(最大65535字节)。过长。加密失败");
                                    return;
                                }
                            }

                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {



                                long fsLength = fs.Length;


                                var bytesOfTitle = ASCIIEncoding.ASCII.GetBytes(MainParameter.Title);
                                var lengthOfTitle = bytesOfTitle.Length;

                                byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                                var lengthOfEdition = edition.Length;


                                //materialForSave = new byte[
                                //    lengthOfTitle
                                //    + lengthOfEdition
                                //    + sFrom.Length
                                //    + 1  //代表文件名字符串长度byte
                                //    + fileNameBytes.Length
                                //    + 2  //代表备注字符串长度，该数字用2byte显示
                                //    + remarksBytes.Length
                                //    + 66 * 256 * HardValue
                                //    + fsLength
                                //    + 32 //后97位为0的情况下的materialForSave hash值
                                //    + 32 //r
                                //    + 32 //s
                                //    + 33 //PublicKey
                                //    ];// 
                                string outPutFilePath = $@"{directoryName}\{fileNameWithoutExtension}.secr";
                                using (FileStream nFile = new FileStream(outPutFilePath, FileMode.Create))
                                {
                                    long position = 0;

                                    //{
                                    //    for (var i = 0; i < bytesOfTitle.Length; i++)
                                    //    {
                                    //        nFile.Seek(position++, SeekOrigin.Begin);
                                    //        nFile.WriteByte()
                                    //    }

                                    //    //bw.Write(bytesOfTitle, position, bytesOfTitle.Length);
                                    //}


                                    //nFile.Seek(position, See)

                                    //for (var i = 0; i < bytesOfTitle.Length; i++)
                                    {
                                        dealWithData(nFile, ref position, bytesOfTitle);
                                        dealWithData(nFile, ref position, edition);
                                        dealWithData(nFile, ref position, sFrom);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(fileNameBytes.Length) });
                                        dealWithData(nFile, ref position, fileNameBytes);
                                        dealWithData(nFile, ref position, new byte[] { Convert.ToByte(remarksBytes.Length / 256), Convert.ToByte(remarksBytes.Length % 256) });
                                        dealWithData(nFile, ref position, remarksBytes);
                                        for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)
                                        {
                                            dealWithData(nFile, ref position, refreData[materialForOrder[indexofM].infomationOfOriginalText]);
                                        }
                                    }
                                    ToJson.Class1.Show(originToSecretCode);
                                    //long position = 66 * 256;
                                    long sum = 0;
                                    using (BinaryReader br = new BinaryReader(fs))
                                    {


                                        for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
                                        {
                                            var sheetIndex = Convert.ToInt32(sum % HardValue);
                                            var byteItem = br.ReadByte();
                                            var secret = originToSecretCode[byteItem][sheetIndex];
                                            var replaceByte = secret;
                                            //materialForSave[position++] = replaceByte;
                                            dealWithData(nFile, ref position, new byte[] { replaceByte });
                                            sum += Convert.ToInt32(byteItem);

                                        }
                                    }
                                    byte[] addBlock = new byte[129];
                                    dealWithData(nFile, ref position, addBlock);
                                    //SHA256 sha256 = new SHA256Managed();
                                    //byte[] hash1 = sha256.ComputeHash(data);
                                    var hashCode = sha256.ComputeHash(nFile);
                                    if (hashCode.Length != 32)
                                    {
                                        output("文件加密不成功！！！hashCode居然不为32");
                                        return;
                                    }
                                    dealWithData(nFile, ref position, hashCode);

                                    //byte[] randomHash;
                                    var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
                                    var r = signal[0];
                                    var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
                                    HexToByteArray.ChangeDirection(ref rByte);

                                    var s = signal[1];
                                    var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
                                    HexToByteArray.ChangeDirection(ref sByte);


                                    dealWithData(nFile, ref position, rByte);

                                    dealWithData(nFile, ref position, sByte);

                                    var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
                                    var sSender = ComPressPublic(publicKeyOfSender);
                                    if (sSender.Length != 33)
                                    {
                                        output("文件加密不成功！！！sSender长度居然不为33");
                                        return;
                                    }

                                    dealWithData(nFile, ref position, sSender);
                                }
                                if (false)
                                {
                                    long position = 0;

                                    for (var i = 0; i < bytesOfTitle.Length; i++)
                                    {
                                        materialForSave[position++] = bytesOfTitle[i];
                                    }

                                    for (var i = 0; i < edition.Length; i++)
                                    {
                                        materialForSave[position++] = edition[i];
                                    }

                                    for (var i = 0; i < sFrom.Length; i++)
                                    {
                                        materialForSave[position++] = sFrom[i];
                                    }

                                    materialForSave[position++] = Convert.ToByte(fileNameBytes.Length);
                                    for (var i = 0; i < fileNameBytes.Length; i++)
                                    {
                                        materialForSave[position++] = fileNameBytes[i];
                                    }

                                    materialForSave[position++] = Convert.ToByte(remarksBytes.Length / 256);
                                    materialForSave[position++] = Convert.ToByte(remarksBytes.Length % 256);
                                    for (var i = 0; i < remarksBytes.Length; i++)
                                    {
                                        materialForSave[position++] = remarksBytes[i];
                                    }

                                    for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)//
                                    {
                                        for (int indexofM2 = 0; indexofM2 < 66; indexofM2++)
                                        {
                                            materialForSave[position++] = refreData[materialForOrder[indexofM].infomationOfSecretText][indexofM2];
                                        }
                                    }
                                    ToJson.Class1.Show(originToSecretCode);
                                    //long position = 66 * 256;
                                    long sum = 0;
                                    using (BinaryReader br = new BinaryReader(fs))
                                    {


                                        for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
                                        {
                                            var sheetIndex = Convert.ToInt32(sum % HardValue);
                                            var byteItem = br.ReadByte();
                                            var secret = originToSecretCode[byteItem][sheetIndex];
                                            var replaceByte = secret;
                                            materialForSave[position++] = replaceByte;
                                            sum += Convert.ToInt32(byteItem);

                                        }
                                    }

                                    //SHA256 sha256 = new SHA256Managed();
                                    //byte[] hash1 = sha256.ComputeHash(data);
                                    var hashCode = sha256.ComputeHash(materialForSave);
                                    if (hashCode.Length != 32)
                                    {
                                        output("文件加密不成功！！！hashCode居然不为32");
                                        return;
                                    }
                                    for (var i = 0; i < hashCode.Length; i++)
                                    {
                                        materialForSave[position++] = hashCode[i];
                                    }

                                    //byte[] randomHash;
                                    var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
                                    var r = signal[0];
                                    var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
                                    HexToByteArray.ChangeDirection(ref rByte);

                                    var s = signal[1];
                                    var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
                                    HexToByteArray.ChangeDirection(ref sByte);

                                    for (var i = 0; i < rByte.Length; i++)
                                    {
                                        materialForSave[position++] = rByte[i];
                                    }
                                    for (var i = 0; i < sByte.Length; i++)
                                    {
                                        materialForSave[position++] = sByte[i];
                                    }

                                    var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
                                    var sSender = ComPressPublic(publicKeyOfSender);
                                    if (sSender.Length != 33)
                                    {
                                        output("文件加密不成功！！！sSender长度居然不为33");
                                        return;
                                    }
                                    for (var i = 0; i < sSender.Length; i++)
                                    {
                                        materialForSave[position++] = sSender[i];
                                    }
                                }
                            }
                        }

                        {
                            //string itemPath = $@"{directoryName}\{fileNameWithoutExtension}.secr";

                            //System.IO.File.WriteAllBytes(itemPath, materialForSave);
                            //output("文件加密成功！！！");
                        }
                    }
                    else
                    {
                        output($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验失败；");
                    }
                }
                else
                {
                    return;
                }
            }
        }

        static void dealWithData(FileStream nFile, ref long position, byte[] bytesOfTitle)
        {
            nFile.Seek(position, SeekOrigin.Begin);
            nFile.Write(bytesOfTitle, 0, bytesOfTitle.Length);
            position += bytesOfTitle.Length;
        }

        /// <summary>
        /// 两个33位 合成66位
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        private static byte[] combineToByte66(byte[] s1, byte[] s2)
        {
            var result = new byte[66];
            for (var i = 0; i < 33; i++)
            {
                result[i] = s1[i];
                result[i + 33] = s2[i];
            }
            return result;
        }
        /// <summary>
        /// 返回33位byte
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] ComPressPublic(BigInteger[] publicKey)
        {
            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            // var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);

            byte[] resultAdd;
            if (publicKey[1].IsEven)
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x02 }, publicKeyArray1);
            else
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x03 }, publicKeyArray1);

            return resultAdd;
            //  Console.WriteLine($"压缩公钥为{ Hex.BytesToHex(resultAdd)}");
        }


        /// <summary>
        /// ComPress 备份
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void ComPress3(inputIntoComPress input, outPutFromComPress output)
        {
            while (true)
            {
                output($"拖入您的Base58编码的37位或38位的私钥的路径，用此私钥进行验证.即校验.txt");

                var privateKeyOfSenderPath = input();



                var privateKeyOfSender = LockAndKeyRead.Get(privateKeyOfSenderPath);
                System.Numerics.BigInteger privateBigInteger;
                if (PrivateKeyF.Check(privateKeyOfSender, out privateBigInteger)) { }
                else
                {
                    output($"请输入正确的私钥！！！");
                    return;
                }

                output("拖入您的输入目标的压缩公钥。即锁.txt");
                var publicKeyPath = input();
                var publicKey = LockAndKeyRead.Get(publicKeyPath);
                var pre = publicKey.Substring(0, 2);
                if (publicKey.Substring(0, 2) == "02" || publicKey.Substring(0, 2) == "03")
                {
                    //原始顺序到密文
                    //Dictionary<int, string> originToSecretText = new Dictionary<int, string>();
                    //Dictionary<string, int> secretTextToOrigin = new Dictionary<string, int>();


                    //Dictionary<string, int> secretTextToSecretCode = new Dictionary<string, int>();

                    //Dictionary<string, string> originalTextToSecretText = new Dictionary<string, string>();
                    //Dictionary<string, string> secretTextToOriginalText = new Dictionary<string, string>();

                    //原码→密码对照表，int，表征sheetNum
                    Dictionary<byte, Dictionary<int, byte>> originToSecretCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //密码→原码对照表，int，表征sheetNum
                    //  Dictionary<byte, Dictionary<int, byte>> secretToOriginCode = new Dictionary<byte, Dictionary<int, byte>>();

                    //压缩公钥和原始数据的对照表
                    //Dictionary<string, byte> msgToByte = new Dictionary<string, byte>();



                    //  压缩公钥和原始数据的对照表
                    Dictionary<string, byte[]> refreData = new Dictionary<string, byte[]>();

                    //用于排序
                    List<InfomationClass> materialForOrder = new List<InfomationClass>();


                    //replace Byte //real To secret real Is Key
                    //Dictionary<byte, List<byte>> replaceBytes = new Dictionary<byte, List<byte>>();

                    //Dictionary<string, string> infomation2ToInfomation = new Dictionary<string, string>();

                    publicKey = publicKey.Substring(2, publicKey.Length - 2);
                    output($"{publicKey}");
                    var X = HexToBigInteger.inputHex(publicKey);
                    var Y = Calculate.GetYByX(X);
                    //if (Calculate.CheckXYIsRight(X, Y))
                    //{ }
                    //else
                    //{
                    //    Console.WriteLine($"请输入正确的公钥");
                    //    return;
                    //}
                    if (pre == "02")
                    {
                        if (Y.IsEven)
                        { }
                        else
                        {
                            Y = (Secp256k1.p - Y);
                        }
                    }
                    else
                    {
                        if (Y.IsEven)
                        {
                            Y = (Secp256k1.p - Y);
                        }
                        else { }
                    }
                    //   Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
                    if (Calculate.CheckXYIsRight(X, Y))
                    {
                        bool isRight;
                        var sFrom = ComPressPublic(new BigInteger[] { X, Y });
                        Deciphering.GetXYByByte33(sFrom, out isRight);
                        if (isRight) { }
                        else
                        {
                            output($"sFrom压缩失败！");
                            return;
                        }

                        //   Console.WriteLine($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验成功；");
                        SHA256 sha256 = new SHA256Managed();

                        output($"输入一个随机数");
                        var xx = input() + DateTime.Now.ToString();
                        byte[] data = Encoding.UTF8.GetBytes(xx);
                        byte[] hash1 = sha256.ComputeHash(data);
                        for (var i = 0; i < 256 * HardValue; i++) //
                        {
                            while (true)
                            {
                                var M = Calculate.getPublicByPrivate(i + 1);
                                var sM = ComPressPublic(M);
                                Deciphering.GetXYByByte33(sM, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"sM压缩失败！");
                                    return;
                                }
                                var infomationOfOriginalText = $"{ Hex.BytesToHex(sM)}";
                                System.Numerics.BigInteger r = Bytes32.ConvetToBigInteger(hash1);
                                hash1 = sha256.ComputeHash(hash1);
                                //  var 
                                var mul1 = Calculate.getMulValue(r, new System.Numerics.BigInteger[] { X, Y });
                                if (mul1 == null)
                                {
                                    continue;
                                }
                                bool isZero;
                                var c1 = Calculate.pointPlus(M, mul1, out isZero);
                                if (c1 == null)
                                {
                                    continue;
                                }
                                var c2 = Calculate.getPublicByPrivate(r);
                                if (c2 == null)
                                {
                                    continue;
                                }
                                var s1 = ComPressPublic(c1);
                                var s2 = ComPressPublic(c2);

                                //bool isRight;
                                Deciphering.GetXYByByte33(s1, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s1压缩失败！");
                                    return;
                                }
                                Deciphering.GetXYByByte33(s2, out isRight);
                                if (isRight) { }
                                else
                                {
                                    output($"s2压缩失败！");
                                    return;
                                }
                                var infomationOfSecretText = $"{ Hex.BytesToHex(s1)}";

                                //output($"{i}压缩公钥为{infomationOfSecretText}");

                                // msgToByte.Add(infomation, Convert.ToByte(i % 256));
                                materialForOrder.Add(new InfomationClass()
                                {
                                    infomationOfSecretText = infomationOfSecretText,
                                    infomationOfOriginalText = infomationOfOriginalText,
                                    responRealByte = Convert.ToByte(i % 256),
                                    step = i / 256,

                                });
                                //infomation2ToInfomation.Add(infomation2)

                                var combineData = combineToByte66(s1, s2);
                                refreData.Add(infomationOfSecretText, combineData);
                                break;
                            }

                        }
                        materialForOrder = materialForOrder.OrderBy(item => item.step).ThenBy(item => item.infomationOfSecretText).ToList();
                        //  materialForOrder = (from item in materialForOrder orderby item.step ascending, item.infomationOfSecretText descending select item).ToList();// orderby item ascending select item).ToList();
                        //  List<int> newOrder = new List<int>();
                        for (var mIndex = 0; mIndex < materialForOrder.Count; mIndex++)
                        {
                            var item = materialForOrder[mIndex];
                            item.secretByte = Convert.ToByte(mIndex % 256);

                            var orginCode = item.responRealByte;

                            if (originToSecretCode.ContainsKey(item.responRealByte))
                            { }
                            else
                            {
                                originToSecretCode.Add(item.responRealByte, new Dictionary<int, byte>());
                            }
                            originToSecretCode[item.responRealByte].Add(item.step, item.secretByte);

                            //if (secretToOriginCode.ContainsKey(item.secretByte)) { }
                            //else
                            //{
                            //    secretToOriginCode.Add(item.secretByte, new Dictionary<int, byte>());
                            //}
                            //secretToOriginCode[item.secretByte].Add(item.step, item.responRealByte);
                        }
                        byte[] materialForSave;

                        string directoryName;
                        string fileNameWithoutExtension;
                        {
                            output("输入路径！！！");
                            string itemPath = input();// $@"F:\工作\201909\DLQU0968.MP4";// Console.ReadLine();//

                            string fileName;
                            byte[] fileNameBytes;

                            {
                                directoryName = System.IO.Path.GetDirectoryName(itemPath);
                                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(itemPath).Trim();//文件名  “Default.aspx”
                                string b = System.IO.Path.GetExtension(itemPath).Trim();//扩展名 “.aspx”
                                                                                        // string mid = string.IsNullOrEmpty(b) ? "" : ".";
                                fileName = $"{fileNameWithoutExtension}{b}";
                                fileNameBytes = UTF8Encoding.UTF8.GetBytes(fileName);
                                if (fileNameBytes.Length >= 256)
                                {
                                    output("文件名(最大255字节)。过长。加密失败");
                                    return;
                                }
                            }

                            string remarks = "";
                            byte[] remarksBytes;
                            {
                                output($"请输入文件的备注(最大65535字节，2W+汉字)。输入空字符串结束！");
                                while (true)
                                {
                                    var inputSelect = input();
                                    if (string.IsNullOrEmpty(inputSelect))
                                    {
                                        output($"输入C/c(continue),继续输入。其他键退出");

                                        var select = input();
                                        if (select.ToUpper() != "C")
                                        {
                                            break;
                                        }
                                    }
                                    remarks = $"{remarks}{inputSelect}{Environment.NewLine}";
                                }
                                remarksBytes = UTF8Encoding.UTF8.GetBytes(remarks);
                                if (remarksBytes.Length >= 256 * 256)
                                {
                                    output("文件备注过程(最大65535字节)。过长。加密失败");
                                    return;
                                }
                            }

                            using (FileStream fs = new FileStream(itemPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
                            {



                                long fsLength = fs.Length;


                                var bytesOfTitle = ASCIIEncoding.ASCII.GetBytes(MainParameter.Title);
                                var lengthOfTitle = bytesOfTitle.Length;

                                byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                                var lengthOfEdition = edition.Length;


                                materialForSave = new byte[
                                    lengthOfTitle
                                    + lengthOfEdition
                                    + sFrom.Length
                                    + 1  //代表文件名字符串长度byte
                                    + fileNameBytes.Length
                                    + 2  //代表备注字符串长度，该数字用2byte显示
                                    + remarksBytes.Length
                                    + 66 * 256 * HardValue
                                    + fsLength
                                    + 32 //后97位为0的情况下的materialForSave hash值
                                    + 32 //r
                                    + 32 //s
                                    + 33 //PublicKey
                                    ];// 
                                long position = 0;

                                for (var i = 0; i < bytesOfTitle.Length; i++)
                                {
                                    materialForSave[position++] = bytesOfTitle[i];
                                }

                                for (var i = 0; i < edition.Length; i++)
                                {
                                    materialForSave[position++] = edition[i];
                                }

                                for (var i = 0; i < sFrom.Length; i++)
                                {
                                    materialForSave[position++] = sFrom[i];
                                }

                                materialForSave[position++] = Convert.ToByte(fileNameBytes.Length);
                                for (var i = 0; i < fileNameBytes.Length; i++)
                                {
                                    materialForSave[position++] = fileNameBytes[i];
                                }

                                materialForSave[position++] = Convert.ToByte(remarksBytes.Length / 256);
                                materialForSave[position++] = Convert.ToByte(remarksBytes.Length % 256);
                                for (var i = 0; i < remarksBytes.Length; i++)
                                {
                                    materialForSave[position++] = remarksBytes[i];
                                }

                                for (int indexofM = 0; indexofM < materialForOrder.Count; indexofM++)//
                                {
                                    for (int indexofM2 = 0; indexofM2 < 66; indexofM2++)
                                    {
                                        materialForSave[position++] = refreData[materialForOrder[indexofM].infomationOfSecretText][indexofM2];
                                    }
                                }
                                ToJson.Class1.Show(originToSecretCode);
                                //long position = 66 * 256;
                                long sum = 0;
                                using (BinaryReader br = new BinaryReader(fs))
                                {


                                    for (long indexOfFs = 0; indexOfFs < fsLength; indexOfFs++)
                                    {
                                        var sheetIndex = Convert.ToInt32(sum % HardValue);
                                        var byteItem = br.ReadByte();
                                        var secret = originToSecretCode[byteItem][sheetIndex];
                                        var replaceByte = secret;
                                        materialForSave[position++] = replaceByte;
                                        sum += Convert.ToInt32(byteItem);

                                    }
                                }

                                //SHA256 sha256 = new SHA256Managed();
                                //byte[] hash1 = sha256.ComputeHash(data);
                                var hashCode = sha256.ComputeHash(materialForSave);
                                if (hashCode.Length != 32)
                                {
                                    output("文件加密不成功！！！hashCode居然不为32");
                                    return;
                                }
                                for (var i = 0; i < hashCode.Length; i++)
                                {
                                    materialForSave[position++] = hashCode[i];
                                }

                                //byte[] randomHash;
                                var signal = PrivateKeyF.Sign(privateBigInteger, hashCode);
                                var r = signal[0];
                                var rByte = HexToByteArray.BigIntegerTo32ByteArray(r);
                                HexToByteArray.ChangeDirection(ref rByte);

                                var s = signal[1];
                                var sByte = HexToByteArray.BigIntegerTo32ByteArray(s);
                                HexToByteArray.ChangeDirection(ref sByte);

                                for (var i = 0; i < rByte.Length; i++)
                                {
                                    materialForSave[position++] = rByte[i];
                                }
                                for (var i = 0; i < sByte.Length; i++)
                                {
                                    materialForSave[position++] = sByte[i];
                                }

                                var publicKeyOfSender = Calculate.getPublicByPrivate(privateBigInteger);
                                var sSender = ComPressPublic(publicKeyOfSender);
                                if (sSender.Length != 33)
                                {
                                    output("文件加密不成功！！！sSender长度居然不为33");
                                    return;
                                }
                                for (var i = 0; i < sSender.Length; i++)
                                {
                                    materialForSave[position++] = sSender[i];
                                }
                            }
                        }

                        {
                            string itemPath = $@"{directoryName}\{fileNameWithoutExtension}.secr";

                            System.IO.File.WriteAllBytes(itemPath, materialForSave);
                            output("文件加密成功！！！");
                        }
                    }
                    else
                    {
                        output($"{ HexToBigInteger.bigIntergetToHex(X) },{ HexToBigInteger.bigIntergetToHex(Y) }校验失败；");
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
