using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Object;

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

            protected override (string, long)[] doLoadObjects(string prefixKey = null)
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

            protected override string[] doTakeObjects(List<string> keyList, string localDirPath)
            {
                try
                {
                    int totalCount = keyList.Count;
                    float completedCount = 0;
                    foreach (var key in keyList)
                    {
                        var count = completedCount;

                        var localFileName = Path.GetFileName(key);
                        var localDir = Path.Combine(localDirPath, Path.GetDirectoryName(key) ?? string.Empty);
                        var request = new GetObjectRequest(this.mBucketName, key, localDir, localFileName);
                        request.SetCosProgressCallback((completed, total) =>
                        {
                            this.updateProgress((count + completed * 100.0f / total) / totalCount);
                        });
                        _cos.GetObject(request);
                        ++completedCount;
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
                    int totalCount = keyList.Count;
                    float completedCount = 0;
                    foreach (var key in keyList)
                    {
                        PutObjectRequest request = new PutObjectRequest(this.mBucketName, key, key);
                        var count = completedCount;
                        request.SetCosProgressCallback(delegate(long completed, long total)
                        {
                            this.updateProgress((count + completed * 100.0f / total) / totalCount);
                        });
                        this._cos.PutObject(request);
                        ++completedCount;
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