using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.NuGet.Asynchronous;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkPatchController<TLocalRepo, TRemoteRepo> : ISnkPatchController
            where TLocalRepo : class, ISnkLocalPatchRepository, new()
            where TRemoteRepo : class, ISnkRemotePatchRepository, new()
        {
            private string _channelName;

            private string _appVersion;

            private TLocalRepo _localRepo;
            private TRemoteRepo _remoteRepo;
            private SnkPatchControlSettings _settings;
            private ISnkJsonParser _jsonParser;

            public virtual string ChannelName => this._channelName;
            public virtual string AppVersion => this._appVersion;
            public virtual SnkPatchControlSettings Settings => this._settings;

            public int LocalResVersion => this._localRepo.Version;
            public int RemoteResVersion => this._remoteRepo.Version;

            public ISnkJsonParser JsonParser => _jsonParser;

            private float prevDownloadProgress;
            private double prevDownloadSize;
            public float DownloadProgress
            {
                get
                {
                    if (prevDownloadSize == _progressPromise.Progress)
                        return prevDownloadProgress;
                    prevDownloadSize = _progressPromise.Progress;
                    prevDownloadProgress = (float)prevDownloadSize / (float)totalDownloadSize;
                    prevDownloadProgress = prevDownloadProgress > 1f ? 1f : prevDownloadProgress;
                    return prevDownloadProgress;
                }
            }


            private ISnkProgressPromise<double> _progressPromise;
            private double totalDownloadSize;

            public SnkPatchController(string channelName, string appVersion, SnkPatchControlSettings settings, ISnkJsonParser jsonParser)
            {
                this._channelName = channelName;
                this._appVersion = appVersion;
                this._settings = settings;
                this._jsonParser = jsonParser;

                this._localRepo = new TLocalRepo();
                this._remoteRepo = new TRemoteRepo();
            }

            public async Task Initialize()
            {
                await this._localRepo.Initialize(this);
                await this._remoteRepo.Initialize(this);
            }

            public async Task Apply(List<SnkSourceInfo> addList, List<string> delList)
            {
                foreach (var key in delList)
                {
                    System.IO.File.Delete(System.IO.Path.Combine(this._localRepo.LocalPath, key));
                }

                for(var i = 0;i<addList.Count;i++)
                {
                    var sourceInfo = addList[i];
                    _remoteRepo.EnqueueDownloadQueue(_localRepo.LocalPath, sourceInfo.key, int.Parse(sourceInfo.version));
                    totalDownloadSize += sourceInfo.size / 1024.0 / 1024.0;
                }

                if (_progressPromise == null)
                    _progressPromise = new SnkProgressResult<double>();

                await _remoteRepo.StartupDownload(_progressPromise);
                _localRepo.UpdateLocalResVersion(_remoteRepo.Version);
            }

            public async Task<(List<SnkSourceInfo>, List<string>)> PreviewDiff(int remoteResVersion = -1)
            {
                if (remoteResVersion < 0)
                    remoteResVersion = this._remoteRepo.Version;
                var remoteManifest = await _remoteRepo.GetSourceInfoList(remoteResVersion);
                var localManifest = await _localRepo.GetSourceInfoList() ?? new List<SnkSourceInfo>();

                return SnkPatch.CompareToDiff(localManifest, remoteManifest);
            }
        }
    }
}