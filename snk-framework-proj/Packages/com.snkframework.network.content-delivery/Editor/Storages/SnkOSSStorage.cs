using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace SnkFramework.Network.ContentDelivery
{
    namespace Editor
    {
        /// <summary>
        /// 阿里云OSS(Object Storage Service)
        /// </summary>
        public class SnkOSSStorage : SnkContentDeliveryStorage<SnkOSSStorageSettings>
        {
            private int mBuffSize => this.settings.mBuffSize;

            private readonly IOss _oss;

            public SnkOSSStorage()
            {
                _oss = new OssClient(this.mEndPoint, this.mAccessKeyId, this.mAccessKeySecret);
            }

            protected override (string, long)[] doLoadObjects(string prefixKey = null)
            {
                var keyList = new List<(string, long)>();
                try
                {
                    ObjectListing result;
                    var request = new ListObjectsRequest(this.mBucketName)
                    {
                        MaxKeys = 1000,
                        Prefix = prefixKey,
                    };

                    do
                    {
                        result = this._oss.ListObjects(request);
                        keyList.AddRange(result.ObjectSummaries.Select(entry => (entry.Key, entry.Size)));
                        request.Marker = result.NextMarker;
                    } while (result.IsTruncated);
                }
                catch (OssException ossException)
                {
                    this.setException(ossException);
                }
                catch (Exception exception)
                {
                    this.setException(exception);
                }

                return keyList.ToArray();
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

                        var request = new GetObjectRequest(this.mBucketName, key);
                        request.StreamTransferProgress += (_, args) =>
                        {
                            this.updateProgress((count + args.TransferredBytes * 100 / (float)args.TotalBytes) /
                                                totalCount);
                        };

                        var ossObject = _oss.GetObject(request);
                        using (var requestStream = ossObject.Content)
                        {
                            var buf = new byte[mBuffSize];
                            string localFilePath = Path.Combine(localDirPath, key);
                            EnsurePathExists(localFilePath);
                            using (var fs = File.Open(localFilePath, FileMode.OpenOrCreate))
                            {
                                var len = 0;
                                while ((len = requestStream.Read(buf, 0, mBuffSize)) != 0)
                                    fs.Write(buf, 0, len);
                                fs.Close();
                            }
                        }

                        ++completedCount;
                    }
                }
                catch (OssException ossException)
                {
                    this.setException(ossException);
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
                        var count = completedCount;
                        using (var fs = File.Open(key, FileMode.Open))
                        {
                            var putObjectRequest = new PutObjectRequest(this.mBucketName, key, fs);
                            putObjectRequest.StreamTransferProgress += (_, args) =>
                            {
                                this.updateProgress((count + args.TransferredBytes * 100 / (float)args.TotalBytes) /
                                                    totalCount);
                            };
                            this._oss.PutObject(putObjectRequest);
                        }

                        ++completedCount;
                    }
                }
                catch (OssException ossException)
                {
                    this.setException(ossException);
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
                    var request = new DeleteObjectsRequest(this.mBucketName, keyList, this.mIsQuietDelete);
                    this._oss.DeleteObjects(request);
                }
                catch (OssException ossException)
                {
                    this.setException(ossException);
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