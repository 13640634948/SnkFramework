using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public interface ISnkPatchController
        {
            string ChannelName { get; }
            string AppVersion { get; }
            int LocalResVersion { get; }
            int RemoteResVersion { get; }
            SnkPatchControlSettings Settings { get; }

            Task Initialize();
            Task<(List<SnkSourceInfo>, List<string>)> PreviewDiff(int remoteResVersion = -1);
            Task Apply(List<SnkSourceInfo> addList, List<string> delList);
        }
    }
}