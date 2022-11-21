using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;
using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Transfer;
using UnityEngine;

namespace SnkFramework.CloudRepository.Editor.Storage
{
    public class SnkRemoteCOSStorageSettings : SnkRemoteStorageSettings
    {
        public long durationSecond;
    }

    public class SnkRemoteCOSStorage : SnkRemoteStorage, ISnkStorageDelete, ISnkStoragePut
    {
        private CosXml _cos;
        private SnkRemoteCOSStorageSettings _settings;

        public SnkRemoteCOSStorage(SnkRemoteCOSStorageSettings settings)
        {
            _settings = settings;

            var config = new CosXmlConfig.Builder()
                .SetRegion(settings.endPoint)
                .Build();

            string secretId = settings.accessKeyId;
            string secretKey = settings.accessKeySecret;
            long durationSecond = _settings.durationSecond;
            var credentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            _cos = new CosXmlServer(config, credentialProvider);
        }

        public override List<SnkStorageObject> LoadObjectList(string path)
        {
            
            try
            {
                var listObjectsRequest = new GetBucketRequest(this._settings.bucketName);
                var result = this._cos.GetBucket(listObjectsRequest);
                //result.listBucket.contentsList;
            }
            catch (CosClientException clientException)
            {
                //Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                //Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
            }
            catch (CosServerException serviceException)
            {
                //Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                //Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
            }

            return default;
        }

        public override void TakeObject(string key, string localPath, SnkStorageTakeOperation takeOperation,
            int buffSize = 2097152)
        {
            Task.Run(() =>
            {
                try
                {
                    string bucket = this._settings.bucketName;
                    string localFileName = Path.GetFileName(localPath);
                    string localDir = Path.GetDirectoryName(localPath);

                    GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
                    request.SetCosProgressCallback((completed, total) =>
                    {
                        takeOperation.progress = completed * 100.0f / total;
                    });
                    _cos.GetObject(request);
                    takeOperation.SetResult();
                }
                catch (COSXML.CosException.CosClientException ex)
                {
                    takeOperation.SetException(ex);
                }
                catch (COSXML.CosException.CosServerException ex)
                {
                    takeOperation.SetException(ex);
                }
                catch (System.Exception ex)
                {
                    takeOperation.SetException(ex);
                }
            });
        }

        public List<string> DeleteObjects(List<string> objectNameList)
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = this._settings.bucketName;
                DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
                //设置返回结果形式
                request.SetDeleteQuiet(false);
                //对象key
                //string key = "exampleobject"; //对象键
                List<string> objects = new List<string>();
                //objects.Add(key);
                objects.AddRange(objectNameList);
                request.SetObjectKeys(objects);
                //执行请求
                DeleteMultiObjectResult result = this._cos.DeleteMultiObjects(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }

            return default;
        }

        public bool PutObjects(string path, List<string> list)
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = this._settings.bucketName;
                string key = path;// "exampleobject"; //对象键
                string srcPath = path;// @"temp-source-file";//本地文件绝对路径

                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult result = this._cos.PutObject(request);
                //对象的 eTag
                string eTag = result.eTag;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }

            return true;
        }
    }
}