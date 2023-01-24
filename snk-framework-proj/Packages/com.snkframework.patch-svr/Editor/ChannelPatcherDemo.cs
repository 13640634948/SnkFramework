using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.NuGet.Basic;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.PatchService
{
    namespace Demo
    {
        internal class SnkCodeGeneratorE : ISnkCodeGenerator
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

        internal class SnkJsonParserE : ISnkJsonParser
        {
            public T FromJson<T>(string json) where T : class => JsonConvert.DeserializeObject<T>(json);

            public string ToJson(object target) => JsonConvert.SerializeObject(target);
        }

        internal class SnkLogger : ISnkLogger
        {
            public void Print(string message)
            {
                Debug.Log(message);
            }
        }
        
        public static class ChannelPatcherDemo
        {
            static string PersistentRepo = "PersistentRepo";
            static string ChannelName = "windf_iOS"; 
            static string AppVersion = "1.0.0";
            private static void internalTest(bool upload = false)
            {
                try
                {
                    NuGet.Snk.Set<ISnkJsonParser>(new SnkJsonParserE());
                    //NuGet.Snk.Set<IEqualityComparer<NuGet.Patch.SnkSourceInfo>>(new SnkSourceInfoComparerE());
                    NuGet.Snk.Set<ISnkCodeGenerator>(new SnkCodeGeneratorE());
                    NuGet.Snk.Set<ISnkLogger>(new SnkLogger());

                    var builder = NuGet.Patch.SnkPatch.CreatePatchBuilder(PersistentRepo, ChannelName, AppVersion);
                    ISnkFileFinder[] sourcePaths =
                    {
                        new SnkFileFinder("ProjectSettingsDemo")
                        {
                            //filters = new [] {"FSTimeGet"},
                            ignores = new[] { ".DS_Store" },
                        },
                    };
                    builder.Build(sourcePaths.ToList());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (upload)
                    Upload();
            }

            [MenuItem("SnkPatcher/Demo-Test")]
            public static void Test()
            {
                internalTest();
            }

            [MenuItem("SnkPatcher/Demo-Test_Upload")]
            public static void Test_Upload()
            {
                internalTest(true);
            }

            [MenuItem("SnkPatcher/Upload")]
            public static void Upload()
            {
                var storage = new SnkCOSStorage();
                var keys = Directory.GetFiles(Path.Combine(PersistentRepo,ChannelName), "*.*", SearchOption.AllDirectories);
                var list = keys.Where(key => !Path.GetFileName(key).StartsWith(".")).ToList();
                storage.PutObjects(list);
            }
        }
    }
}