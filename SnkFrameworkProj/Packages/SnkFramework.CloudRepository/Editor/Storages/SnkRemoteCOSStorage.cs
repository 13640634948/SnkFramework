using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public bool PutObjects(string path, List<string> list)
        {
            throw new System.NotImplementedException();
        }
    }
}