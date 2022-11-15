using SnkFramework.PatchBuilder.Runtime;
using SnkFramework.PatchBuilder.Runtime.Base;
using UnityEngine;

namespace SnkFramework.PatchBuilder
{
    namespace Demo
    {
        public class ChannelPatcherDemo : MonoBehaviour
        {
            private string repoName = "windf_iOS";
            private ChannelPatchBuilder channelPatcher;

            void Start()
            {
                var sourcePaths = new SourcePath[]
                {
                    new() { sourceDirPath = "Temp" },
                };

                channelPatcher = ChannelPatchBuilder.Load(repoName);
                var patcher = channelPatcher.Build(sourcePaths);
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}