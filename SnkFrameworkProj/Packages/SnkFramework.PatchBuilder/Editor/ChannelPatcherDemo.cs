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
            private ChannelPatchBuilder channelPatcher;

            void Start()
            {
                var sourcePaths = new SourceFinder[]
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
