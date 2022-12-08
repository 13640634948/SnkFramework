using System;
using System.Collections.Generic;
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

            protected override (string, long)[] doLoadObjects(string prefixKey = null)
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

            protected override string[] doTakeObjects(List<string> keyList, string localDirPath)
            {
                try
                {
                    int totalCount = keyList.Count;
                    float completedCount = 0;
                    foreach (var key in keyList)
                    {
                        var count = completedCount;
                        var request = new GetObjectRequest
                        {
                            BucketName = this.mBucketName,
                            ObjectKey = key
                        };
                        request.DownloadProgress += (_, status) =>
                            this.updateProgress((count + status.TransferPercentage) / totalCount);
                        var response = _obs.GetObject(request);
                        response.WriteResponseStreamToFile(System.IO.Path.Combine(localDirPath, request.ObjectKey));
                        ++completedCount;
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
                    int totalCount = keyList.Count;
                    float completedCount = 0;
                    foreach (var key in keyList)
                    {
                        var count = completedCount;
                        var request = new PutObjectRequest
                        {
                            BucketName = this.mBucketName,
                            ObjectKey = key,
                            FilePath = key,
                        };
                        request.UploadProgress += (_, status) =>
                            this.updateProgress((count + status.TransferPercentage) / totalCount);
                        _obs.PutObject(request);
                        ++completedCount;
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