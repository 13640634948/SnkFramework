using System;
using System.Linq;
using SnkFramework.PatchService.Runtime;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    private ISnkPatchService _patchService;
    public async void Start()
    {
        PatchSettings settings = new PatchSettings();
        settings.repoRootPath = "PersistentDataPath";
        settings.channelName = "windf_iOS";
        _patchService = SnkPatchService.CreatePatchService(settings);
        try
        {
            await _patchService.Initialize();
            Debug.Log("init - finish");
            var isLatestVersion = _patchService.IsLatestVersion();
            Debug.Log("isLatestVersion:" + isLatestVersion);
            if(isLatestVersion)
                return;
        
            
            var diffManifest = await _patchService.PreviewPatchSynchronyPromise();
            Debug.Log("count:" + diffManifest.addList.Count + ", size:" + diffManifest.addList.Sum(a=>a.size));
            _patchService.ApplyPatch(diffManifest);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
