using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OBS;
using OBS.Model;

namespace SnkFramework.Network.ContentDelivery
{
    namespace Editor
    {
        /// <summary>
        /// 华为云OBS(Object Storage Service)
        /// </summary>
        public class SnkOBSStorage : SnkContentDeliveryStorage<SnkOBSStorageSettings>
        {
            private readonly ObsClient _obs;

            public SnkOBSStorage()
            {
                _obs = new ObsClient(mAccessKeyId, mAccessKeySecret, mEndPoint);
            }

            protected override IEnumerable<(string, long)> doLoadObjects(string prefixKey = null)
            {
                var keyList = new List<(string, long)>();
                try
                {
                    ListObjectsResponse response;
                    var request = new ListObjectsRequest
                    {
                        BucketName = this.mBucketName,
                        MaxKeys = 1000,
                        Prefix = prefixKey,
                    };

                    do
                    {
                        response = this._obs.ListObjects(request);
                        keyList.AddRange(response.ObsObjects.Select(entry => (entry.ObjectKey, entry.Size)));
                        request.Marker = response.NextMarker;
                    } while (response.IsTruncated);
                }
                catch (ObsException obsException)
                {
                    this.setException(obsException);
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
                    var request = new GetObjectRequest
                    {
                        BucketName = this.mBucketName,
                        ObjectKey = key
                    };
                    request.DownloadProgress += (_, status) =>
                        this.onProgressCallbackHandle?.Invoke(key, status.TransferredBytes, status.TotalBytes, 1, 1);
                    var response = _obs.GetObject(request);

                    var buffer = new byte[8192];
                    using (var memStream = new MemoryStream())
                    {
                        var total = 0;
                        var count=0;
                        while ((count = response.OutputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memStream.Write(buffer, 0, count);
                            total += count;
                        }

                        buffer = new byte[total];
                        var resultLen = memStream.Read(buffer, 0, buffer.Length);
                        if (resultLen != response.ContentLength)
                            throw new ObsException(
                                $"The total bytes read {resultLen} from response stream is not equal to the Content-Length {response.ContentLength}", ErrorType.Receiver,
                                null);
                        return buffer;
                    }
                }
                catch (ObsException obsException)
                {
                    this.setException(obsException);
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
                        var request = new GetObjectRequest
                        {
                            BucketName = this.mBucketName,
                            ObjectKey = keyList[i]
                        };
                        request.DownloadProgress += (_, status) =>
                            this.onProgressCallbackHandle?.Invoke(keyList[i], status.TransferredBytes, status.TotalBytes, i, keyList.Count);

                        var response = _obs.GetObject(request);
                        response.WriteResponseStreamToFile(System.IO.Path.Combine(localDirPath, request.ObjectKey));
                    }
                }
                catch (ObsException obsException)
                {
                    this.setException(obsException);
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
                        var request = new PutObjectRequest
                        {
                            BucketName = this.mBucketName,
                            ObjectKey = keyList[i],
                            FilePath = keyList[i],
                        };
                        request.UploadProgress += (_, status) =>
                            this.onProgressCallbackHandle?.Invoke(keyList[i], status.TransferredBytes, status.TotalBytes, i, keyList.Count);
                        _obs.PutObject(request);
                    }
                }
                catch (ObsException obsException)
                {
                    this.setException(obsException);
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
                    var request = new DeleteObjectsRequest
                    {
                        BucketName = this.mBucketName,
                        Quiet = this.mIsQuietDelete,
                    };
                    foreach (var str in keyList)
                        request.AddKey(str);
                    this._obs.DeleteObjects(request);
                }
                catch (ObsException obsException)
                {
                    this.setException(obsException);
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