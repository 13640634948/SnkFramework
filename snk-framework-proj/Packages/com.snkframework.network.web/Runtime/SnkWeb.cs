using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace SnkFramework.Network.Web
{
    public class SnkWeb
    {
        public static IEnumerator HttpGet(string uri, System.Action<bool, string> onCompleted)
        {
            using var webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.Success)
                onCompleted?.Invoke(true,webRequest.downloadHandler.text);
            else
                onCompleted?.Invoke(false,webRequest.error);
        }
        public static IEnumerator HttpPost(string uri, Dictionary<string,string> formDict, System.Action<bool, string> onCompleted)
        {
            WWWForm form = new WWWForm();
            foreach (var kvp in formDict)
                form.AddField(kvp.Key, kvp.Value);

            using var webRequest = UnityWebRequest.Post(uri,form);
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.Success)
                onCompleted?.Invoke(true,webRequest.downloadHandler.text);
            else
                onCompleted?.Invoke(false,webRequest.error);
        }

        public static IEnumerator HttpDownload(string uri, string dirPath, System.Action<bool> onCompleted)
        {
            using var webRequest = UnityWebRequest.Get(uri);
            string fileName = Path.GetFileName(uri);
            string localFilePath = Path.Combine(dirPath, fileName);
            webRequest.downloadHandler = new DownloadHandlerFile(localFilePath);
            yield return webRequest.SendWebRequest();
            onCompleted?.Invoke(webRequest.result == UnityWebRequest.Result.Success);
        }
    }
}