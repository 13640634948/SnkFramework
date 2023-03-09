using System.Text;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.HttpWeb;
using SnkFramework.Runtime.Engine;
using UnityEngine;
using UnityEngine.UI;

namespace SnkSamples.Snk_HttpService
{
    public class HttpTest : MonoBehaviour
    {
        public Slider slider;

        private ISnkDownloadTask task;

        private float progress;

        public void Start()
        {
            SnkLogHost.Registry(new SnkUnityLogFactory());
        }

        public void StartDownload()
        {
            var savePath = Application.dataPath + "/../test.apk";
            var downloadUri =
                "http://10.20.204.3:8888/admin/resmgr/inner-down-file/apk/inner/windf_channel1023_inner_yw_bVersion0_bNumber124_bTypeDebug_20221023_172338.apk";

            task = SnkHttpDownloadController.CreateDownloadTask(downloadUri, savePath);
            task.SetDownloadFormBreakpoint(true);
            Debug.LogError("开始下载");
            SnkHttpDownloadController.Implement(task);
        }

        public void StopDownload()
        {
            Debug.LogError("取消下载");
            task?.CancelDownload();
        }

        public async void TestHead()
        {
            //var uri = "http://10.20.204.3:8888/admin/resmgr/inner-down-file/JumpLink/JumpLink.json";
            var uri = "https://hero-halo-windf.oss-cn-shenzhen.aliyuncs.com/JumpLink/JumpLink.json";
            var result = await SnkHttpWeb.Head(uri);
            if (result.isError)
            {
                Debug.LogError(result.errorMessage);
                return;
            }

            Debug.LogError(result.length);
        }

        public async void TestGet()
        {
            var uri = "http://10.20.204.3:8888/admin/resmgr/inner-down-file/JumpLink/JumpLink.json";
            var result = await SnkHttpWeb.Get(uri);
            if (result.isError)
            {
                Debug.LogError(result.errorMessage);
            }

            Debug.LogError("result  " + result.isDone);
            var content = UTF8Encoding.UTF8.GetString(result.data);
            Debug.LogError(content);
        }

        public void Update()
        {
            if (task == null) return;
            progress = task.GetDownloadProgress();
            slider.value = progress;
        }

        public void OnDestroy()
        {
            task?.CancelDownload();
        }
    }
}