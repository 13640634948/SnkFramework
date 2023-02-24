using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Asynchronous;
using SnkFramework.NuGet.Features.HttpWeb;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkRemotePatchRepository : ISnkRemotePatchRepository
        {
            private readonly double SizeUnit = 1024.0 * 1024.0;

            public int Version { get; protected set; }

            protected ISnkPatchController _patchCtrl;

            private SnkVersionInfos _versionInfos;

            private int _urlIndex;

            private int _maxThreadNumber = 5;
            private int _threadTickInterval = 100;
            private double currDownloadedSize;

            private Dictionary<string, ISnkDownloadTask> DownloadTaskDict = new Dictionary<string, ISnkDownloadTask>();
            private Queue<ISnkDownloadTask> willDownloadTaskQueue = new Queue<ISnkDownloadTask>();
            private List<(string, ISnkDownloadTask)> downloadingTaskList = new List<(string, ISnkDownloadTask)>();


            public async Task Initialize(ISnkPatchController patchController)
            {
                try
                {
                    //SnkNuget.Logger?.Info("RemoteRepo.Initialize:");
                    this._patchCtrl = patchController;
                    string basicURL = getCurrURL();
                    //SnkNuget.Logger?.Info("basicURL:" + basicURL);
                    //SnkNuget.Logger?.Info("_patchCtrl-1.ChannelName:" + _patchCtrl.ChannelName);
                    //SnkNuget.Logger?.Info("_patchCtrl-2.AppVersion:" + _patchCtrl.AppVersion);
                    //SnkNuget.Logger?.Info("_patchCtrl-3.Settings:" + _patchCtrl.Settings);
                    //SnkNuget.Logger?.Info("_patchCtrl-4.Settings.versionInfoFileName:" + _patchCtrl.Settings.versionInfoFileName);
                    string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, _patchCtrl.Settings.versionInfoFileName);
                    //SnkNuget.Logger?.Info("url:" + url);

                    var result = await SnkHttpWeb.Get(url);
                    if (result.isError)
                    {
                        throw new AggregateException("获取远端版本信息失败。URL:" + url + "\nerrText:" + result.errorMessage);
                    }
                    var content = UTF8Encoding.UTF8.GetString(result.data);

                    _versionInfos = this._patchCtrl.JsonParser.FromJson<SnkVersionInfos>(content);
                    SnkNuget.Logger?.Info($"[RemoteInit]AppVersion:{_versionInfos.appVersion}");
                    foreach (var a in _versionInfos.histories)
                    {
                        SnkNuget.Logger?.Info($"[RemoteInit]AppVersion:{a.version}|{a.size}|{a.count}|{a.code}");
                    }
                    Version = _versionInfos.histories[_versionInfos.histories.Count - 1].version;
                    SnkNuget.Logger?.Info($"[RemoteInit]Version:{Version}");

                }
                catch (System.Exception exception)
                {
                    SnkNuget.Logger?.Error("[Exception]\n" + exception.Message + "\n" + exception.StackTrace);
                }
            }

            public List<SnkVersionMeta> GetResVersionHistories()=> this._versionInfos.histories;

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
                SnkNuget.Logger?.Info("url:" + url);

                var content = UTF8Encoding.UTF8.GetString(result.data);
                return this._patchCtrl.JsonParser.FromJson<List<SnkSourceInfo>>(content);
            }

            public void SetMaxThreadNumber(int maxThreadNumber)
            {
                this._maxThreadNumber = maxThreadNumber <= 0 ? 1 : maxThreadNumber;
            }

            public void SetThreadTickInterval(int threadTickInterval)
            {
                this._threadTickInterval = threadTickInterval;
            }

            public void EnqueueDownloadQueue(string dirPath, string key, int resVersion)
            {
                string basicURL = getCurrURL();
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, resVersion.ToString(), _patchCtrl.Settings.assetsDirName, key);
                var task = SnkHttpDownloadController.CreateDownloadTask(url, Path.Combine(dirPath, key));
                task.SetDownloadFormBreakpoint(true);
                willDownloadTaskQueue.Enqueue(task);
                DownloadTaskDict.Add(task.URL, task);
            }

            private bool isDone()
            {
                foreach (var a in this.DownloadTaskDict)
                {
                    if (a.Value.DownloadResult.isDone)
                        continue;
                    return false;
                }
                return true;
            }


            void refreshDownloadProgress(ISnkProgressPromise<double> progressPromise)
            {
                currDownloadedSize = this.DownloadTaskDict.Sum(a => (a.Value.DownloadedSize / SizeUnit));
                progressPromise.UpdateProgress(currDownloadedSize);
            }

            public async Task StartupDownload(ISnkProgressPromise<double> progressPromise)
            {
                await Task.Run(() =>
                {
                    while (this.isDone() == false)
                    {
                        downloadingTaskList.RemoveAll(a => a.Item2.DownloadResult.isDone);

                        while(downloadingTaskList.Count < this._maxThreadNumber && willDownloadTaskQueue.Count > 0)
                        {
                            var task = willDownloadTaskQueue.Dequeue();
                            downloadingTaskList.Add(new ValueTuple<string, ISnkDownloadTask>(task.URL, task));
                            SnkHttpDownloadController.Implement(task);
                        }
                        System.Threading.Thread.Sleep(_threadTickInterval);
                        this.refreshDownloadProgress(progressPromise);
                    }
                });
            }
        }
    }
}