using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OBS;
using OBS.Model;
using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;
using UnityEngine;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Storage
    {
        /// <summary>
        /// 华为云OBS(Object Storage Service)
        /// </summary>
        public class SnkRemoteOBSStorage : SnkRemoteStorage, ISnkStorageDelete, ISnkStoragePut
        {
            private ObsClient _obs;
            private SnkRemoteStorageSettings _settings;

            public SnkRemoteOBSStorage(SnkRemoteStorageSettings settings)
            {
                _settings = settings;
                _obs = new ObsClient(_settings.accessKeyId, _settings.accessKeySecret, _settings.endPoint);
            }

            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                try
                {
                    ListObjectsRequest request = new ListObjectsRequest();
                    ListObjectsResponse response;
                    request.BucketName = this._settings.bucketName;
                    request.MaxKeys = 1000;
                    request.Prefix = string.Empty;//文件前缀
                    do
                    {
                        response = this._obs.ListObjects(request);
                        foreach (ObsObject entry in response.ObsObjects)
                        {
                            //Console.WriteLine("key = {0} size = {1}", entry.ObjectKey, entry.Size);
                        }
                        request.Marker = response.NextMarker;
                    }
                    while (response.IsTruncated);
                }
                catch (ObsException ex)
                {
                    //Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                    //Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
                }

                return default;
            }

            public override void TakeObject(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 1024 * 1024 * 2)
            {
                try
                {
                    GetObjectRequest request = new GetObjectRequest();
                    request.DownloadProgress += (_, status) => takeOperation.progress = status.TransferPercentage;
                    request.BucketName = this._settings.bucketName;
                    request.ObjectKey = key;

                    _obs.BeginGetObject(request, ar =>
                    {
                        try
                        {
                            using var response = _obs.EndGetObject(ar);
                            CleanPath(localPath);
                            response.WriteResponseStreamToFile(localPath);
                            takeOperation.SetResult();
                        }
                        catch (ObsException ex)
                        {
                            takeOperation.SetException(ex);
                        }
                    }, null);
                }
                catch (ObsException ex)
                {
                    takeOperation.SetException(ex);
                }
            }

            public List<string> DeleteObjects(List<string> objectNameList)
            {
                try
                {
                    DeleteObjectsRequest request = new DeleteObjectsRequest();
                    request.BucketName = "bucketname";
                    request.Quiet = true;
                    //request.AddKey("objectname1");
                    //request.AddKey("objectname2");
                    DeleteObjectsResponse response = this._obs.DeleteObjects(request);
                    //Console.WriteLine("Delete objects response: {0}", response.StatusCode);
                    return default;
                }
                catch (ObsException ex)
                {
                    throw ex;
                    //Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                    //Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
                } 
            }

            public bool PutObjects(string path, List<string> list)
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = this._settings.bucketName,//待传入目标桶名
                    ObjectKey = path,   //待传入对象名(不是本地文件名，是文件上传到桶里后展现的对象名)
                    FilePath = path,//待上传的本地文件路径，需要指定到具体的文件名
                };
                PutObjectResponse response = this._obs.PutObject(request);
                //Console.WriteLine("put object response: {0}", response.StatusCode);
                return true;
            }
        }
    }
}