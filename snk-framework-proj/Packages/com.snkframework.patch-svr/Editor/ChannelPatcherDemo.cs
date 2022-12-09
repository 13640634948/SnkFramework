using System.Collections.Generic;
using System.IO;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.PatchService.Editor;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Base;
using UnityEditor;

namespace SnkFramework.PatchService
{
    namespace Demo
    {
        static public class ChannelPatcherDemo
        {
            private static void internalTest(bool force, bool upload = false)
            {
                string repoName = "windf_iOS";
                SnkPatchBuilder snkPatcher;
                var sourcePaths = new SnkSourceFinder[]
                {
                    new()
                    {
                        sourceDirPath = "ProjectSettings",
                        //filters = new [] {"FSTimeGet"},
                        //ignores = new [] {"aebe", "407"},
                    },
                };

                snkPatcher = SnkPatchBuilder.Load(repoName);
                snkPatcher.Build(sourcePaths, force);
                if (upload)
                {
                    SnkCOSStorage storage = new SnkCOSStorage();
                    string[] keys = Directory.GetFiles(Path.Combine(SNK_BUILDER_CONST.REPO_ROOT_PATH,repoName), "*.*", SearchOption.AllDirectories);
                    storage.PutObjects(new List<string>(keys));
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