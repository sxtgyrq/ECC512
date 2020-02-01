using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CheckOnly
    {
        public static void Check()
        {
            Console.WriteLine("输入文件夹，如F:\\生活\\ysecret");
            DirectoryInfo root = new DirectoryInfo(Console.ReadLine());
            FileInfo[] files = root.GetFiles();

            Dictionary<string, string> md5s = new Dictionary<string, string>();
            for (int indexOfFileForCompressing = 0; indexOfFileForCompressing < files.Length; indexOfFileForCompressing++)
            {
                //bool isRight;
                Console.WriteLine($"{files[indexOfFileForCompressing].FullName}");
                var md5 = GetMD5HashFromFile(files[indexOfFileForCompressing].FullName);
                if (md5s.ContainsKey(md5))
                {
                    Console.WriteLine("检验到文件冲突");
                    Console.WriteLine(md5s[md5]);
                    Console.WriteLine(files[indexOfFileForCompressing].FullName);
                    return;
                }
                else
                {
                    md5s.Add(md5, files[indexOfFileForCompressing].FullName);
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
    }
}
