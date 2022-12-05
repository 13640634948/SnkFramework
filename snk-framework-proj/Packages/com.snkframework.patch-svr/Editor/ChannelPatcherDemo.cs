using SnkFramework.PatchBuilder.Editor;
using SnkFramework.PatchBuilder.Runtime.Base;
using UnityEditor;

namespace SnkFramework.PatchBuilder
{
    namespace Demo
    {
        static public class ChannelPatcherDemo
        {
            private static void internalTest(bool force)
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
        }
    }
}