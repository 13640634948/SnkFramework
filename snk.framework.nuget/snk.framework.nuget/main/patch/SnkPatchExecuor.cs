using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snk.framework.nuget
{
    namespace patch
    {
        public class SnkPatchExecuor
        {
            private ISnkLocalPatchRepository _localRepo;
            private ISnkRemotePatchRepository _remoteRepo;

            public SnkPatchExecuor(ISnkLocalPatchRepository localRepo = null, ISnkRemotePatchRepository remoteRepo = null)
            {
                this._localRepo = localRepo;
                this._remoteRepo = remoteRepo;
            }

            public static async Task Apply(
                ISnkLocalPatchRepository localRepo, 
                ISnkRemotePatchRepository remoteRepo, 
                System.Tuple<List<SnkSourceInfo>, List<string>> diffTuple,
                string assetRootDirName = "assets")
            {
                var localPath = System.IO.Path.Combine(localRepo.LocalPath, assetRootDirName);
                foreach (var filePath in diffTuple.Item2)
                {
                    System.IO.File.Delete(filePath);
                }

                foreach (var sourceInfo in diffTuple.Item1)
                {
                    await remoteRepo.TakeFileToLocal(localPath, sourceInfo.key, int.Parse(sourceInfo.version));
                }

                localRepo.UpdateLocalResVersion(remoteRepo.Version);
            }

            public static async Task<(List<SnkSourceInfo>, List<string>)> PreviewDiff(
                ISnkLocalPatchRepository localRepo,
                ISnkRemotePatchRepository remoteRepo)
            {
                var comparer = Snk.Get<IEqualityComparer<SnkSourceInfo>>();
                var remoteManifest = await remoteRepo.GetSourceInfoList();
                var localManifest = await localRepo.GetSourceInfoList();
                var addList = remoteManifest.Except(localManifest, comparer).ToList();
                var delList = localManifest.Except(remoteManifest, comparer).Select(a => a.key).ToList();
                return (addList, delList);
            }
        }
    }
}