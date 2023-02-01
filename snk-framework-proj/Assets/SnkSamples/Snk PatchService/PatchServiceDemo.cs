using System;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime.Basic;
using UnityEngine;

public class SnkLoggerProvider : ISnkLoggerProvider
{
    public void Output(eSnkLogType logType, Exception e, string formater, params object[] message)
    {
        if(e != null)
            Debug.LogException(e);
        else
        {
            switch (logType)
            {
                case eSnkLogType.error:
                    Debug.LogErrorFormat(formater, message);
                    break;
                case eSnkLogType.warning:
                    Debug.LogWarningFormat(formater, message);
                    break;
                default:
                    Debug.LogFormat(formater, message);
                    break;
            }
        }
    }
}

public class PatchServiceDemo : MonoBehaviour
{
    public async void Start()
    {
        SnkNuget.Logger = new SnkLogger("asdf", new SnkLoggerProvider());

        var settings = new SnkPatchControlSettings
        {
            remoteURLs = new[]
            {
                "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PersistentRepo"
            },
            localPatchRepoPath = "PersistentAssets/assets"
        };

        try
        {
            var patchController = SnkPatch.CreatePatchExecuor<SnkJsonParser>("windf_iOS", "1.0.0", settings);
            await patchController.Initialize();
            Debug.Log("LocalResVersion:" + patchController.LocalResVersion);
            Debug.Log("RemoteResVersion:" + patchController.RemoteResVersion);

            if (patchController.LocalResVersion >= patchController.RemoteResVersion) //本地版本 >= 远端版本
                return;
            var (addList, delList) = await patchController.PreviewDiff();
            await patchController.Apply(addList, delList);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}