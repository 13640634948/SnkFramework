using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.HttpWeb;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkRemotePatchRepository : ISnkRemotePatchRepository
        {
            public int Version { get; protected set; }

            protected ISnkPatchController _patchCtrl;

            private SnkVersionInfos _versionInfos;

            private int _urlIndex;

            public async Task Initialize(ISnkPatchController patchController)
            {
                this._patchCtrl = patchController;
                string basicURL = getCurrURL();
                Snk.Logger?.Info("basicURL:" + basicURL);
                Snk.Logger?.Info("_patchCtrl.ChannelName:" + _patchCtrl.ChannelName);
                Snk.Logger?.Info("_patchCtrl.AppVersion:" + _patchCtrl.AppVersion);
                Snk.Logger?.Info("_patchCtrl.Settings.versionInfoFileName:" + _patchCtrl.Settings.versionInfoFileName);
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, _patchCtrl.Settings.versionInfoFileName);
                var (result, json) = await SnkHttpWeb.HttpGet(url);
                if (result == false)
                {
                    throw new AggregateException("获取远端版本信息失败。URL:" + url);
                }
                _versionInfos = this._patchCtrl.JsonParser.FromJson<SnkVersionInfos>(json);
                Version = _versionInfos.histories[_versionInfos.histories.Count - 1].version;
            }

            public List<SnkVersionMeta> GetResVersionHistories()
                => this._versionInfos.histories;

            private string getCurrURL(bool moveNext = false)
            {
                if (moveNext)
                {
                    var result = _urlIndex + 1 >= this._patchCtrl.Settings.remoteURLs.Length;
                    _urlIndex = result ? 0 : (_urlIndex + 1);
                }
                return this._patchCtrl.Settings.remoteURLs[_urlIndex];
            }

            public async Task<List<SnkSourceInfo>> GetSourceInfoList(int version)
            {
                string basicURL = getCurrURL();
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, version.ToString(), _patchCtrl.Settings.manifestFileName);
                var (result, json) = await SnkHttpWeb.HttpGet(url);
                if (result == false)
                {
                    throw new AggregateException("获取远端版本信息失败。URL:" + url);
                }
                return this._patchCtrl.JsonParser.FromJson<List<SnkSourceInfo>>(json);
            }

            public async Task TakeFileToLocal(string dirPath, string key, int resVersion)
            {
                string basicURL = getCurrURL();
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, resVersion.ToString(), _patchCtrl.Settings.assetsDirName, key);
                var result = await SnkHttpWeb.HttpDownload(url, Path.Combine(dirPath, key));
                if (result == false)
                    throw new Exception("[Download-Error]" + url);
            }

        }
    }
}