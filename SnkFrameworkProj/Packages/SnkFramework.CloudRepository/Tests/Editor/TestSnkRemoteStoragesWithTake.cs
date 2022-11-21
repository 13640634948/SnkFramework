using System;
using System.Collections;
using SnkFramework.CloudRepository.Editor.Storage;
using SnkFramework.CloudRepository.Runtime.Base;
using UnityEngine;
using UnityEngine.TestTools;

namespace SnkFramework.CloudRepository.Tests
{
    namespace Editor
    {
        public class TestSnkRemoteStoragesWithTake
        {
            public static Func<SnkRemoteCOSStorageSettings> mRemoteCOSSettingsGetter = null;
            public static Func<SnkRemoteStorageSettings> mRemoteCBSSettingsGetter = null;
            public static Func<SnkRemoteStorageSettings> mRemoteOSSSettingsGetter = null;
            
            private IEnumerator ExecuteTest(ISnkRemoteStorage storage, string key, string localPath)
            {
                var takeOperation = new SnkStorageTakeOperation();
                storage.TakeObject(key, localPath, takeOperation);
                int old = 0;
                
                while (takeOperation.isCompleted == false)
                {
                    //Debug.Log("progress:" + takeOperation.progress);
                    int tmp = (int)takeOperation.progress;
                    if (tmp != old)
                    {
                        old = tmp;
                        Debug.Log("progress:" + (int)takeOperation.progress);
                    }
                    yield return null;
                }

                if (takeOperation.exception != null)
                {
                    Debug.LogException(takeOperation.exception);
                    yield break;
                }

                Debug.Log("finish:" + takeOperation.progress);
            }

            [UnityTest] //腾讯云对象仓库测试
            public IEnumerator SnkRemoteCOSStorageWithEnumeratorPasses()
            {
                var settings = mRemoteCOSSettingsGetter.Invoke();
                if (mRemoteCOSSettingsGetter == null || settings == null)
                    throw new ArgumentNullException(nameof(mRemoteCOSSettingsGetter));

                string key = "SnkCloudRepository/ios_root/patcher.zip";
                string localPath = "SnkCloudRepository_COS/ios_root/patcher.zip";
                var storage = new SnkRemoteCOSStorage(settings);
                yield return ExecuteTest(storage, key, localPath);
            }

            [UnityTest] //阿里云对象仓库测试
            public IEnumerator SnkRemoteOSSStorageWithEnumeratorPasses()
            {
                var settings = mRemoteOSSSettingsGetter.Invoke();
                if (mRemoteCOSSettingsGetter == null || settings == null)
                    throw new ArgumentNullException(nameof(mRemoteOSSSettingsGetter));
                
                string key = "SnkCloudRepository/ios_root/patcher.zip";
                string localPath = "SnkCloudRepository_OSS/ios_root/patcher.zip";
                var storage = new SnkRemoteOSSStorage(settings);
                yield return ExecuteTest(storage, key, localPath);
            }

            [UnityTest] //华为云对象仓库测试
            public IEnumerator SnkRemoteOBSStorageWithEnumeratorPasses()
            {
                var settings = mRemoteCBSSettingsGetter.Invoke();
                if (mRemoteCOSSettingsGetter == null || settings == null)
                    throw new ArgumentNullException(nameof(mRemoteCBSSettingsGetter));
                
                string key = "SnkCloudRepository/ios_root/patcher.zip";
                string localPath = "SnkCloudRepository_OBS/ios_root/patcher.zip";
                var storage = new SnkRemoteOBSStorage(settings);
                yield return ExecuteTest(storage, key, localPath);
            }
        }
    }
}