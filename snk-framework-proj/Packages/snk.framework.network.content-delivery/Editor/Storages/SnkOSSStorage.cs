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

            protected override IEnumerable<(string, long)> doLoadObjects(string prefixKey = null)
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

            protected override IEnumerable<byte> doTakeObject(string key)
            {
                try
                {
                    var request = new GetObjectRequest(this.mBucketName, key);
                    request.StreamTransferProgress += (_, args) =>
                    {
                        this.onProgressCallbackHandle?.Invoke(key, args.TransferredBytes, args.TotalBytes, 1, 1);
                    };

                    var ossObject = _oss.GetObject(request);
                    var buffer = new byte[mBuffSize];
                    using (var requestStream = ossObject.Content)
                    {
                        var total = 0;
                        using (var memStream = new MemoryStream())
                        {
                            var len = 0;
                            while ((len = requestStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                memStream.Write(buffer, 0, len);
                                total += len;
                            }
                            buffer = new byte[total];
                            var resultLen = memStream.Read(buffer, 0, buffer.Length);
                            if (resultLen != ossObject.ContentLength)
                            {    throw new OssException(
                                    $"The total bytes read {resultLen} from response stream is not equal to the Content-Length {ossObject.ContentLength}, ErrType:Receiver",
                                    null);
                            }
                            return buffer;
                        }
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
                return null;
            }

            protected override string[] doTakeObjects(List<string> keyList, string localDirPath)
            {
                try
                {
                    for (var i = 0; i < keyList.Count; i++)
                    {
                        var key = keyList[i];
                        var request = new GetObjectRequest(this.mBucketName, key);
                        request.StreamTransferProgress += (_, args) =>
                        {                            
                            this.onProgressCallbackHandle?.Invoke(key, args.TransferredBytes,
                                args.TotalBytes, i, keyList.Count);

                        };

                        var ossObject = _oss.GetObject(request);
                        using (var requestStream = ossObject.Content)
                        {
                            var buf = new byte[mBuffSize];
                            var localFilePath = Path.Combine(localDirPath, key);
                            EnsurePathExists(localFilePath);
                            using (var fs = File.Open(localFilePath, FileMode.OpenOrCreate))
                            {
                                var len = 0;
                                while ((len = requestStream.Read(buf, 0, mBuffSize)) != 0)
                                    fs.Write(buf, 0, len);
                                fs.Close();
                            }
                        }
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
                    for (var i = 0; i < keyList.Count; i++)
                    {
                        var key = keyList[i];
                        using (var fs = File.Open(key, FileMode.Open))
                        {
                            var putObjectRequest = new PutObjectRequest(this.mBucketName, key, fs);
                            putObjectRequest.StreamTransferProgress += (_, args) =>
                            {
                                this.onProgressCallbackHandle?.Invoke(key, args.TransferredBytes, args.TotalBytes, i, keyList.Count);
                            };
                            this._oss.PutObject(putObjectRequest);
                        }
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