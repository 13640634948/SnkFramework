using System;
using System.Linq;
using SnkFramework.PatchService.Runtime;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    private ISnkPatchService _patchService;
    public async void Start()
    {
        _patchService = new SnkPatchService();
        try
        {
            var settings = new SnkPatchSettings
            {
                appVersion = "0.0.2",
                repoRootPath = "PersistentAssets",
                channelName = "windf_iOS",
                versionFileName = "version.txt"
            };
            
            await _patchService.Initialize(settings);
            Debug.Log("init - finish");

            var versionCompare = _patchService.LocalResVersion.CompareTo(_patchService.RemoteResVersion);
            Debug.Log("versionCompare:" + versionCompare);
            if(versionCompare >= 0)//本地版本 >= 远端版本
                return;
            var diffManifest = await _patchService.PreviewPatchSynchronyPromise();
            Debug.Log("count:" + diffManifest.addList.Count + ", size:" + diffManifest.addList.Sum(a=>a.size));
            await _patchService.ApplyDiffManifest(diffManifest);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}