using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Core;
using UnityEngine;

public class PatchServiceDemo : MonoBehaviour
{
    private SnkPatchService _patchService;
    // Start is called before the first frame update
    public async void Start()
    {
        _patchService = new SnkPatchService();
        await _patchService.Initialize();
        IEnumerable<int> unupgradedList = null;
        bool isLatestVersion = _patchService.IsLatestVersion(ref unupgradedList);
        if (isLatestVersion)
            return;
        var list = unupgradedList.ToList();
        int from = list[0];
        int to = list[list.Count - 1];
        var diffManifest = await _patchService.CollectUpgradSourceInfoList(from, to);
        var promise = _patchService.PreviewPatchSynchronyPromise(diffManifest);
        _patchService.ApplyPatch(promise);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
