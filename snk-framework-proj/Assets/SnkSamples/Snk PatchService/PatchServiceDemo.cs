using System;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Patch;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    public async void Start()
    {
        Snk.Set<ISnkJsonParser>(new SnkJsonParser());
        Snk.Set<ISnkCodeGenerator>(new SnkCodeGenerator());
        Snk.Set<ISnkLogger>(new SnkLogger());

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
            var patchController = SnkPatch.CreatePatchExecuor("windf_iOS", "1.0.0", settings);
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