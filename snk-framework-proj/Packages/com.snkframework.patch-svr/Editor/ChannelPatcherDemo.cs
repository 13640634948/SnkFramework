using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.PatchService.Editor;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Base;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.PatchService
{
    namespace Demo
    {
        public static class ChannelPatcherDemo
        {
            static string ChannelName = "windf_iOS";

            private static void internalTest(bool upload = false)
            {
                SnkPatchBuilder snkPatcher;
                var sourcePaths = new SnkSourceFinder[]
                {
                    new()
                    {
                        sourceDirPath = "ProjectSettingsDemo",
                        //filters = new [] {"FSTimeGet"},
                        ignores = new [] {".DS_Store"},
                    },
                };

                SnkPatchBuilder.Build(ChannelName, Version.Parse("0.0.2"), sourcePaths);

                if (upload)
                    Upload();
            }

            [MenuItem("SnkPatcher/Demo-Test")]
            public static void Test()
            {
                internalTest();
            }
            
            [MenuItem("SnkPatcher/Demo-Test_Upload")]
            public static void Test_Upload()
            {
                internalTest(true);
            }
            [MenuItem("SnkPatcher/Upload")]
            public static void Upload()
            {                
                var storage = new SnkCOSStorage();
                var keys = Directory.GetFiles(Path.Combine(SNK_BUILDER_CONST.REPO_ROOT_PATH,ChannelName), "*.*", SearchOption.AllDirectories);
                var list = keys.Where(key => !Path.GetFileName(key).StartsWith(".")).ToList();
                storage.PutObjects(list);
            }
            
        }
    }
}