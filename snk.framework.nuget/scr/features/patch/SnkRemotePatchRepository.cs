using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SnkFramework.NuGet.Asynchronous;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.HttpWeb;
using static SnkFramework.NuGet.Features.HttpWeb.SnkDownloadTask;

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
                    Version = _versionInfos.histories[_versionInfos.histories.Count - 1].version;
                }
                catch (System.Exception exception)
                {
                    SnkNuget.Logger?.Error("[Exception]\n" + exception.Message + "\n" + exception.StackTrace);
                }
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
                SnkNuget.Logger?.Info("url:" + url);

                var content = UTF8Encoding.UTF8.GetString(result.data);
                return this._patchCtrl.JsonParser.FromJson<List<SnkSourceInfo>>(content);
            }

            private Dictionary<string, ISnkDownloadTask> DownloadTaskDict = new Dictionary<string, ISnkDownloadTask>();
            private Queue<ISnkDownloadTask> willDownloadTaskQueue = new Queue<ISnkDownloadTask>();
            private List<(string, ISnkDownloadTask)> downloadingTaskList = new List<(string, ISnkDownloadTask)>();

            public int MaxThreadNumber = 5;
            public void EnqueueDownloadQueue(string dirPath, string key, int resVersion)
            {
                string basicURL = getCurrURL();
                string url = Path.Combine(basicURL, _patchCtrl.ChannelName, _patchCtrl.AppVersion, resVersion.ToString(), _patchCtrl.Settings.assetsDirName, key);
                var task = SnkHttpDownloadController.CreateDownloadTask(url, Path.Combine(dirPath, key));

                SnkNuget.Logger?.Info("[EnQue]" + task.URL);
                task.SetDownloadFormBreakpoint(true);
                var downloadResultTask = SnkHttpDownloadController.Implement(task);
                var tuple = new ValueTuple<string, ISnkDownloadTask>(task.URL, task);
                downloadingTaskList.Add(tuple);
                //willDownloadTaskQueue.Enqueue(task);
                DownloadTaskDict.Add(task.URL, task);
            }

            private int downloadedTaskCount = 0;
            private DownloadProgress downloadProgress = new DownloadProgress();

            //progressPromise.UpdateProgress(downloadProgress);

            private bool IsDone()
            {
                foreach (var a in this.DownloadTaskDict)
                {
                    if (a.Value.DownloadResult.isDone)
                        continue;
                    return false;
                }
                return true;
            }

            public void StartupDownload(ISnkProgressPromise<DownloadProgress> progressPromise)
            {
                /*
                while (willDownloadTaskQueue.Count > 0)
                {
                    var task = willDownloadTaskQueue.Dequeue();
                    SnkNuget.Logger?.Info("[DownloadTask]" + task.URL);
                    var downloadResultTask = SnkHttpDownloadController.Implement(task);
                    var tuple = new ValueTuple<string, ISnkDownloadTask, SnkHttpDownloadResult>(task.URL, task, downloadResultTask.Result);
                    downloadingTaskList.Add(tuple);
                }
                */

                Task.Run(() =>
                {
                    float downloadedCount = 0;
                    while (this.IsDone() == false)
                    {
                        for (int i = 0; i < downloadingTaskList.Count; i++)
                        {
                            if (downloadingTaskList[i].Item2.DownloadResult.isDone)
                            {
                                downloadingTaskList.RemoveAt(i--);
                            }
                        }

                        if (downloadingTaskList.Count < MaxThreadNumber)
                        {
                            if (willDownloadTaskQueue.Count > 0)
                            {
                                var task = willDownloadTaskQueue.Dequeue();
                                var downloadResultTask = SnkHttpDownloadController.Implement(task);
                                var tuple = new ValueTuple<string, ISnkDownloadTask>(task.URL, task);
                                downloadingTaskList.Add(tuple);
                            }
                        }
                    }
                });

                /*
                SnkNuget.Logger?.Info("[StartupDownload]");
                await Task.Run(() => {
                    int totalTaskCount = DownloadTaskDict.Count;
                    while (downloadedTaskCount < totalTaskCount)
                    {
                        //移除已经下载完成的任务
                        for (int i = 0; i < downloadingTaskList.Count; i++)
                        {
                            var tuple = downloadingTaskList[i];
                            if (tuple.Item3.isDone == true)
                            {
                                downloadingTaskList.RemoveAt(i--);
                                downloadedTaskCount++;
                            }
                        }

                        //开启新的任务
                        if (downloadingTaskList.Count < MaxThreadNumber)
                        {
                            if (willDownloadTaskQueue.Count > 0)
                            {
                                var task = willDownloadTaskQueue.Dequeue();
                                SnkNuget.Logger?.Info("[DownloadTask]" + task.URL);
                                var downloadResultTask = SnkHttpDownloadController.Implement(task);
                                var tuple = new ValueTuple<string, ISnkDownloadTask, SnkHttpDownloadResult>(task.URL, task, downloadResultTask.Result);
                                downloadingTaskList.Add(tuple);
                            }
                        }

                        float progress = downloadedTaskCount;
                        string outputString = string.Empty;
                        for (int i = 0; i < downloadingTaskList.Count; i++)
                        {
                            var tuple = downloadingTaskList[i];
                            progress += tuple.Item2.GetDownloadProgress();
                        }
                        SnkNuget.Logger?.Info($"[progress]{progress}/{totalTaskCount}\n" + outputString);
                        downloadProgress.progress = progress / totalTaskCount;
                        progressPromise.UpdateProgress(downloadProgress);
                    }
                });
                */
            }
        }
    }
}