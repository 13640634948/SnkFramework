using System;
using System.Collections;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.LayoutEngine.UGUI;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SampleDevelop.Mvvm
{
    public class MvvmLoader : IMvvmLoader
    {
        public ISnkViewOwner LoadViewOwner(string ownerPath)
        {
            Debug.Log(ownerPath);
            GameObject asset = Resources.Load<GameObject>(ownerPath);
            GameObject inst = GameObject.Instantiate(asset);
            return inst.AddComponent<UGUIViewOwner>();
        }

        public IEnumerator LoadViewOwnerAsync(string ownerPath, Action<ISnkViewOwner> callback)
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(ownerPath);
            yield return request;
            GameObject inst = GameObject.Instantiate(request.asset as GameObject);
            callback.Invoke(inst.AddComponent<UGUIViewOwner>());
        }
    }
}