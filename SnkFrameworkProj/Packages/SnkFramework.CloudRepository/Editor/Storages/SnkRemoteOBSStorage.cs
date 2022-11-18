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
                Debug.Log("SnkRemoteOSSStorage-Ctor");
                _settings = settings;
                //var obsCfg = new ObsConfig();
                //obsCfg.Endpoint = _settings.endPoint;
                _obs = new ObsClient(_settings.accessKeyId, _settings.accessKeySecret, _settings.endPoint);
            }

            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new System.NotImplementedException();
            }


            public override void TakeObjects(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 1024 * 1024 * 2)
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
                throw new System.NotImplementedException();
            }

            public bool PutObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}