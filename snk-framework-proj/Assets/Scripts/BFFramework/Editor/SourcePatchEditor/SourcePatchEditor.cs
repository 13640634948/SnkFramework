using System;
using System.Collections.Generic;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime.Basic;
using SnkFramework.Runtime.Engine;
using UnityEditor;
using UnityEngine;

namespace BFFramework.Editor
{
    public class SourcePatchEditorMenu
    {
        [MenuItem("Window/SnkFramework/PatchBuild")]
        public static void SyncToRemote()
        {
            try
            {
                SnkLogHost.Registry(new SnkUnityLogFactory());
                const string projPath = "PatchRepo";
                const string channelName = "back_fire";
                const string appVersion = "1.0.0";
                var settings = new SnkPatchSettings();
                var builder = SnkPatch.CreatePatchBuilder<SnkJsonParser>(projPath, channelName, appVersion, settings);
            
                builder.Build(new List<ISnkFileFinder>()
                {
                    new SnkFileFinder("Repository")
                    {
                        //filters = new[]{""},
                        ignores = new[]{".DS_Store", ".snk_manifest"},
                    }
                });
                SnkAssetSyncMenu.SyncToRemote(projPath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        
        }
    }
}