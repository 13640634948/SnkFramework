using System.Collections.Generic;
using System.Threading.Tasks;
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

            public float ApplyProgress => 0;

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

                foreach (var sourceInfo in addList)
                {
                    await _remoteRepo.TakeFileToLocal(_localRepo.LocalPath, sourceInfo.key, int.Parse(sourceInfo.version));
                }
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