using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.Network.Web;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Core;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    private const string ROOTPATH = "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PatcherRepository";

    public SnkDiffManifest diffManifest;
    public List<SnkSourceInfo> localList;
    public List<SnkSourceInfo> remoteList;
    private ISnkPatchService _patchService;
    public int SourceRollBackToVersion = -1;
    public async void Start()
    {
        var settings = new PatchSettings
        {
            repoRootPath = "PersistentDataPath",
            channelName = "windf_iOS"
        };
        
        _patchService = SnkPatchService.CreatePatchService(settings);
        try
        {
            await _patchService.Initialize();
            Debug.Log("init - finish");

            if (SourceRollBackToVersion >= 0)
            {
                diffManifest = await _patchService.PreviewRepairSourceToLatestVersion();
            }
            else
            {
                var isLatestVersion = _patchService.IsLatestVersion();
                Debug.Log("isLatestVersion:" + isLatestVersion);
                if(isLatestVersion)
                    return;
                var diffManifest = await _patchService.PreviewPatchSynchronyPromise();
                Debug.Log("count:" + diffManifest.addList.Count + ", size:" + diffManifest.addList.Sum(a=>a.size));
                _patchService.ApplyDiffManifest(diffManifest);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
