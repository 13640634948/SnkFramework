using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SnkFramework.PatchService.Runtime.Base;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class PatchHelper
    {
        
        /// <summary>
        /// 获取版本目录名
        /// </summary>
        /// <returns>版本目录名</returns>
        public static string GetVersionDirectoryName(int version) => version.ToString();

        public static IEqualityComparer<SnkSourceInfo> comparer = new SnkSourceInfoComparer();

        /// <summary>
        /// 生成资源信息列表
        /// </summary>
        /// <param name="version">信息列表的版本号</param>
        /// <param name="sourceFinder">资源探测器</param>
        /// <returns>资源信息列表</returns>
        public static List<SnkSourceInfo> GenerateSourceInfoList(int resVersion, ISnkSourceFinder sourceFinder)
        {
            var result = sourceFinder.TrySurvey(out var fileInfos, out var dirFullPath);
            if (result == false)
                return null;

            return fileInfos.Select(fileInfo => new SnkSourceInfo
            {
                name = fileInfo.FullName.Replace(dirFullPath, string.Empty).Substring(1),
                version = resVersion,
                md5 = getMD5ByMD5CryptoService(fileInfo.FullName),
                size = fileInfo.Length,
            }).ToList();
        }

        /// <summary>
        /// 复制资源文件到指定目录下
        /// </summary>
        /// <param name="toDirectoryFullPath">目标目录的绝对路径</param>
        /// <param name="sourceInfoList">需要复制的资源信息列表</param>
        public static void CopySourceTo(string toDirectoryFullPath, List<SnkSourceInfo> sourceInfoList)
        {
            foreach (var sourceInfo in sourceInfoList)
            {
                var fromFileInfo = new FileInfo(sourceInfo.name);
                var toFileInfo = new FileInfo(Path.Combine(toDirectoryFullPath, sourceInfo.name));
                if (toFileInfo.Directory!.Exists == false)
                    toFileInfo.Directory.Create();
                fromFileInfo.CopyTo(toFileInfo.FullName);
            }
        }
        
        /// <summary>
        /// 文件生成MD5（适用小文件）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getMD5ByMD5CryptoService(string path)        
        {            
            if (!File.Exists(path)) return "";            
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);            
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();            
            byte[] buffer = md5Provider.ComputeHash(fs);            
            string resule = BitConverter.ToString(buffer).Replace("-", string.Empty);
            md5Provider.Clear();            
            fs.Close();           
            return resule;        
        }
        
        /// <summary>
        /// 文件生成MD5（适用大文件）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getMD5ByHashAlgorithm(string path)        
        {            
            if (!File.Exists(path)) return "";            
            int bufferSize = 1024 * 16;//自定义缓冲区大小16K            
            byte[] buffer = new byte[bufferSize];            
            Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);           
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();            
            int readLength = 0;//每次读取长度            
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
    }
}