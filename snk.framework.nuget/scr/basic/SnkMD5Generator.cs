using System;
using System.IO;
using System.Security.Cryptography;
using SnkFramework.NuGet.Basic;
namespace SnkFramework.NuGet
{
    namespace Basic
    {
        public class SnkMD5Generator : ISnkCodeGenerator
        {
            /// <summary>
            /// 文件生成MD5（适用大文件）
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public string GetMD5ByHashAlgorithm(string filePath)
            {
                if (!File.Exists(filePath)) return "";
                int bufferSize = 1024 * 16; //自定义缓冲区大小16K            
                byte[] buffer = new byte[bufferSize];
                Stream inputStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
                int readLength = 0; //每次读取长度            
                var output = new byte[bufferSize];
                while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //计算MD5                
                    hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
                }

                //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)            		  
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
                string md5 = BitConverter.ToString(hashAlgorithm.Hash);
                hashAlgorithm.Clear();
                inputStream.Close();
                return md5.Replace("-", string.Empty);
            }

            /// <summary>
            /// 文件生成MD5（适用小文件）
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public string GetMD5ByMD5CryptoService(string filePath)
            {
                if (!File.Exists(filePath)) return "";
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                byte[] buffer = md5Provider.ComputeHash(fs);
                string resule = BitConverter.ToString(buffer).Replace("-", string.Empty);
                md5Provider.Clear();
                fs.Close();
                return resule;
            }
        }
    }
}