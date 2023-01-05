using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.PatchService.Editor;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Base;
using UnityEditor;

namespace SnkFramework.PatchService
{
    namespace Demo
    {
        public static class ChannelPatcherDemo
        {
            private static void internalTest(bool force, bool upload = false)
            {
                string repoName = "windf_iOS";
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

                snkPatcher = SnkPatchBuilder.Load(repoName, Version.Parse("0.0.2"));
                snkPatcher.Build(sourcePaths, force);
                if (upload)
                {
                    var storage = new SnkCOSStorage();
                    var keys = Directory.GetFiles(Path.Combine(SNK_BUILDER_CONST.REPO_ROOT_PATH,repoName), "*.*", SearchOption.AllDirectories);
                    var list = keys.Where(key => !Path.GetFileName(key).StartsWith(".")).ToList();
                    storage.PutObjects(list);
                }
            }

            [MenuItem("SnkPatcher/Demo-TestWeak")]
            public static void TestWeak()
            {
                internalTest(false);
            }
            
            [MenuItem("SnkPatcher/Demo-TestForce")]
            public static void TestForce()
            {
                internalTest(true);
            }
            
            [MenuItem("SnkPatcher/Demo-TestWeak_Upload")]
            public static void TestWeak_Upload()
            {
                internalTest(false,true);
            }
            
            [MenuItem("SnkPatcher/Demo-TestForce_Upload")]
            public static void TestForce_Upload()
            {
                internalTest(true,true);
            }
        }
    }
}