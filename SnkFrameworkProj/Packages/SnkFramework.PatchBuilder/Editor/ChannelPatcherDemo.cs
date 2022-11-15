/*
using SnkFramework.PatchBuilder.Editor;
using SnkFramework.PatchBuilder.Runtime.Base;
using UnityEngine;

namespace SnkFramework.PatchBuilder
{
    namespace Demo
    {
        public class ChannelPatcherDemo : MonoBehaviour
        {
            private string repoName = "windf_iOS";
            private SnkPatchBuilder snkPatcher;

            void Start()
            {
                var sourcePaths = new SnkSourceFinder[]
                {
                    new() { sourceDirPath = "Temp" },
                };

                snkPatcher = SnkPatchBuilder.Load(repoName);
                var patcher = snkPatcher.Build(sourcePaths);
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}
*/