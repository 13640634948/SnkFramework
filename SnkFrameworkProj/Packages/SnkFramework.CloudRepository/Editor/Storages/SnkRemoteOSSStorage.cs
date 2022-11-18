using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using CodiceApp.EventTracking;
using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;
using UnityEngine;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Storage
    {
        /// <summary>
        /// 阿里云OSS(Object Storage Service)
        /// </summary>
        public class SnkRemoteOSSStorage : SnkRemoteStorage, ISnkStorageDelete, ISnkStoragePut
        {
            private IOss _oss;
            private SnkRemoteStorageSettings _settings;

            public SnkRemoteOSSStorage(SnkRemoteStorageSettings settings)
            {
                _settings = settings;
                _oss = new OssClient(settings.endPoint, settings.accessKeyId, settings.accessKeySecret);
            }

            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new System.NotImplementedException();
            }

            protected void CleanPath(string fullPath)
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
            }

            public override void TakeObjects(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 2097152)
            {
                try
                {
                    var request = new GetObjectRequest(this._settings.bucketName, key);
                    
                    request.StreamTransferProgress += (_, args) =>
                    {
                        takeOperation.progress = args.TransferredBytes * 100 / (float)args.TotalBytes;
                    };
                    
                    _oss.BeginGetObject(request, ar =>
                    {
                        try
                        {
                            using var ossObject = _oss.EndGetObject(ar);
                            using (var requestStream = ossObject.Content)
                            {
                                byte[] buf = new byte[buffSize];
                                CleanPath(localPath);
                                using (var fs = File.Open(localPath, FileMode.OpenOrCreate))
                                {
                                    var len = 0;
                                    while ((len = requestStream.Read(buf, 0, buffSize)) != 0)
                                        fs.Write(buf, 0, len);
                                    fs.Close();
                                }
                                takeOperation.SetResult();
                            }
                        }
                        catch (Exception e)
                        {
                            takeOperation.SetException(e);
                        }
                    }, null);
                }
                catch (OssException ex)
                {
                    takeOperation.SetException(ex);
                }
                catch (Exception ex)
                {
                    takeOperation.SetException(ex);
                }
            }


            public List<string> DeleteObjects(List<string> objectNameList)
            {
                throw new System.NotImplementedException();
            }

            public bool PutObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}