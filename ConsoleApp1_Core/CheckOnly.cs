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

            var files = getAll(Console.ReadLine());
            //DirectoryInfo root = new DirectoryInfo();
            //FileInfo[] files = root.GetFiles();

            Dictionary<string, string> md5s = new Dictionary<string, string>();
            for (int indexOfFileForCompressing = 0; indexOfFileForCompressing < files.Count; indexOfFileForCompressing++)
            {
                //bool isRight;
                Console.WriteLine($"{files[indexOfFileForCompressing]}");
                var md5 = GetMD5HashFromFile(files[indexOfFileForCompressing]);
                if (md5s.ContainsKey(md5))
                {
                    Console.WriteLine("检验到文件冲突");
                    Console.WriteLine(md5s[md5]);
                    Console.WriteLine(files[indexOfFileForCompressing]);
                    return;
                }
                else
                {
                    md5s.Add(md5, files[indexOfFileForCompressing]);
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


        public static void Check2()
        {
            Console.WriteLine("输入文件夹，如F:\\生活\\ysecret");
            var path = Console.ReadLine();
            getAll(path);
        }
        static List<string> getAll(string path)
        {
            List<string> result = new List<string>();

            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            for (int indexOfFileForCompressing = 0; indexOfFileForCompressing < files.Length; indexOfFileForCompressing++)
            {
                //bool isRight;
                //  Console.WriteLine($"{files[indexOfFileForCompressing].FullName}");
                result.Add(files[indexOfFileForCompressing].FullName);
            }
            var directories = root.GetDirectories();

            for (var i = 0; i < directories.Length; i++)
            {
                var m = getAll(directories[i].FullName);
                for (var j = 0; j < m.Count; j++)
                {
                    result.Add(m[j]);
                }
            }

            return result;
        }
    }
}
