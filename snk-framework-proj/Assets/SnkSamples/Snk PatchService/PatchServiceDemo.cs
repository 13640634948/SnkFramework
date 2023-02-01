using System;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime.Basic;
using SnkFramework.Runtime.Engine;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    public async void Start()
    {
        SnkNuget.Logger = new SnkLogger("asdf", new UnityLoggerProvider());

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