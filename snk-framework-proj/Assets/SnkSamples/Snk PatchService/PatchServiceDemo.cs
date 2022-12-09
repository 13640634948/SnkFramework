using System;
using SnkFramework.PatchService.Runtime;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    private ISnkPatchService _patchService;
    public async void Start()
    {
        PatchSettings settings = new PatchSettings();
        _patchService = SnkPatchService.CreatePatchService(settings);
        try
        {
            await _patchService.Initialize();
            Debug.Log("init - finish");
            var (isLatestVersion, diffManifest) = await _patchService.IsLatestVersion();
            Debug.Log("isLatestVersion:" + isLatestVersion);
            if(isLatestVersion)
                return;
        
            var promise = _patchService.PreviewPatchSynchronyPromise(diffManifest);
            Debug.Log("count:" + promise.SourceInfoList.Count + ", size:" + promise.GetTotalSize());
            _patchService.ApplyPatch(promise);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
