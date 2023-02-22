using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
                //SnkNuget.Logger?.Info("basicURL:" + basicURL);
                //SnkNuget.Logger?.Info("_patchCtrl.ChannelName:" + _patchCtrl.ChannelName);
                //SnkNuget.Logger?.Info("_patchCtrl.AppVersion:" + _patchCtrl.AppVersion);
                //SnkNuget.Logger?.Info("_patchCtrl.Settings.versionInfoFileName:" + _patchCtrl.Settings.versionInfoFileName);
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, _patchCtrl.Settings.versionInfoFileName);
                var result= await SnkHttpWeb.Get(url);
                if (result.isError)
                {
                    throw new AggregateException("获取远端版本信息失败。URL:" + url + "\nerrText:" + result.errorMessage);
                }
                var content = UTF8Encoding.UTF8.GetString(result.data);
                _versionInfos = this._patchCtrl.JsonParser.FromJson<SnkVersionInfos>(content);
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
                var result = await SnkHttpWeb.Get(url);
                if (result.isError == true)
                {
                    throw new AggregateException("获取远端版本信息失败。URL:" + url + "\nerrText:" + result.errorMessage);
                }
                var content = UTF8Encoding.UTF8.GetString(result.data);
                return this._patchCtrl.JsonParser.FromJson<List<SnkSourceInfo>>(content);
            }

            public async Task TakeFileToLocal(string dirPath, string key, int resVersion)
            {
                string basicURL = getCurrURL();
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, resVersion.ToString(), _patchCtrl.Settings.assetsDirName, key);
                var task = SnkHttpDownloadController.CreateDownloadTask(url, Path.Combine(dirPath, key));
                task.SetDownloadFormBreakpoint(true);
                 var result = await SnkHttpDownloadController.Implement(task);
                if (result.isError)
                    throw new Exception("[Download-Error]" + url + "\nerrText:" + result.errorMessage);
            }

        }
    }
}