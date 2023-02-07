using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.HttpWeb;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Runtime.Engine;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SnkSamples.Snk_HttpService
{
    public class HttpTest : MonoBehaviour
    {
        public Slider slider;

        private SnkDownloadTask task;

        private float progress;

        public void StartDownload()
        {
            SnkNuget.Logger = new SnkLogger("HttpTest", new SnkUnityLoggerProvider());
            var savePath = Application.dataPath + "/../aa.apk";
            var downloadUri =
                "http://10.20.204.3:8888/admin/resmgr/inner-down-file/%E5%AE%89%E8%A3%85%E5%8C%85/%E7%89%88%E7%BD%B2%E5%8C%85/WindFantasy_VersionName-banshu_BaseVersion-14_BuildVersion-14_VersionCode-156_Svn-13758_Time-20210511180636_release_outer.apk";
            var downloadParam = new DownloadParam();
            downloadParam.uri = downloadUri;
            downloadParam.savePath = savePath;
            downloadParam.downloadFormBreakpoint = true;
            downloadParam.progressCallback = (downloadedSize, totalSize) =>
            {
                progress = (float) downloadedSize / totalSize;
            };
            task = new SnkDownloadTask(downloadParam);
            Debug.LogError("开始下载");
            SnkHttpDownloadImplementer.Implement(task);
        }

        public void StopDownload()
        {
            Debug.LogError("取消下载");
            task?.CancelDownload();
        }

        public async void TestHead()
        {
            var uri =
                "http://10.20.204.3:8888/admin/resmgr/inner-down-file/%E5%AE%89%E8%A3%85%E5%8C%85/%E7%89%88%E7%BD%B2%E5%8C%85/WindFantasy_VersionName-banshu_BaseVersion-14_BuildVersion-14_VersionCode-156_Svn-13758_Time-20210511180636_release_outer.apk";
            var result = await SnkHttpWeb.Head(uri, 5000);
            Debug.LogError(result.length);
        }

        public async void TestGet()
        {
            var uri = "http://10.20.204.3:8888/admin/resmgr/inner-down-file/JumpLink/JumpLink.json";
            var result = await SnkHttpWeb.Get(uri, 2000);
            Debug.LogError(result.errorMessage);
            var content = UTF8Encoding.UTF8.GetString(result.data);
            Debug.LogError(content);
         
        }

        public void Update()
        {
            slider.value = progress;
        }
    }
}