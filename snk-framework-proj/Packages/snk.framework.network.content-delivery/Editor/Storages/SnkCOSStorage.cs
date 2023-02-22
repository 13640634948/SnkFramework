using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.Network.ContentDelivery
{
    namespace Editor
    {
        public class SnkCOSStorage : SnkContentDeliveryStorage<SnkCOSStorageSettings>
        {
            private readonly CosXml _cos;
            private long mDurationSecond => this.settings.mDurationSecond;

            public SnkCOSStorage()
            {
                var config = new CosXmlConfig.Builder()
                    .SetRegion(this.mEndPoint)
                    .Build();

                var credentialProvider = new DefaultQCloudCredentialProvider(
                    this.mAccessKeyId,
                    this.mAccessKeySecret,
                    this.mDurationSecond);

                _cos = new CosXmlServer(config, credentialProvider);
            }

            protected override IEnumerable<(string, long)> doLoadObjects(string prefixKey = null)
            {
                (string, long)[] resultArray = null;
                try
                {
                    var request = new GetBucketRequest(this.mBucketName);
                    if (string.IsNullOrEmpty(prefixKey) == false)
                        request.SetPrefix(prefixKey);
                    var result = this._cos.GetBucket(request);
                    resultArray = result.listBucket.contentsList.Select(a => (a.key, a.size)).ToArray();
                }
                catch (CosClientException clientException)
                {
                    this.setException(clientException);
                }
                catch (CosServerException serviceException)
                {
                    this.setException(serviceException);
                }
                catch (Exception exception)
                {
                    this.setException(exception);
                }

                return resultArray;
            }
            
            protected override IEnumerable<byte> doTakeObject(string key)
            {
                try
                {
                    var request = new GetObjectBytesRequest(mBucketName, key);
                    //设置进度回调
                    request.SetCosProgressCallback(delegate (long completed, long total)
                    {
                        this.onProgressCallbackHandle?.Invoke(key, completed, total, 1, 1);
                    });
                    //执行请求
                    var result = _cos.GetObject(request);
                    return result.content;
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

                return null;
            }
            

            protected override string[] doTakeObjects(List<string> keyList, string localDirPath)
            {
                try
                {
                    for (var i = 0; i < keyList.Count; i++)
                    {
                        var key = keyList[i];
                        var localFileName = Path.GetFileName(key);
                        var localDir = Path.Combine(localDirPath, Path.GetDirectoryName(key) ?? string.Empty);
                        var request = new GetObjectRequest(this.mBucketName, key, localDir, localFileName);
                        request.SetCosProgressCallback((completed, total) =>
                        {
                            this.onProgressCallbackHandle?.Invoke(key, completed, total,i, keyList.Count);
                        });
                        _cos.GetObject(request);
                    }
                }
                catch (CosClientException clientException)
                {
                    this.setException(clientException);
                }
                catch (CosServerException serviceException)
                {
                    this.setException(serviceException);
                }
                catch (Exception exception)
                {
                    this.setException(exception);
                }

                return keyList.ToArray();
            }

            protected override string[] doPutObjects(List<string> keyList)
            {
                try
                {
                    for (var i = 0; i < keyList.Count; i++)
                    {
                        var key = keyList[i];
                        var request = new PutObjectRequest(this.mBucketName, key, key);
                        request.SetCosProgressCallback(delegate(long completed, long total)
                        {
                            this.onProgressCallbackHandle.Invoke(key, completed, total,i, keyList.Count);
                        });
                        this._cos.PutObject(request);
                    }
                }
                catch (CosClientException clientException)
                {
                    this.setException(clientException);
                }
                catch (CosServerException serviceException)
                {
                    this.setException(serviceException);
                }
                catch (Exception exception)
                {
                    this.setException(exception);
                }

                return keyList.ToArray();
            }

            protected override string[] doDeleteObjects(List<string> keyList)
            {
                try
                {
                    var request = new DeleteMultiObjectRequest(this.mBucketName);
                    request.SetDeleteQuiet(this.mIsQuietDelete);
                    request.SetObjectKeys(keyList);
                    this._cos.DeleteMultiObjects(request);
                }
                catch (CosClientException clientException)
                {
                    this.setException(clientException);
                }
                catch (CosServerException serviceException)
                {
                    this.setException(serviceException);
                }
                catch (Exception exception)
                {
                    this.setException(exception);
                }

                return keyList.ToArray();
            }
        }
    }
}