using System;
using System.Collections;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class ResourceUILocator : UILocator, IResourceUILocator
{
    protected readonly string UI_PREFAB_PATH_FORMAT = "UI/Prefabs/{0}";

    public override TView LoadView<TView>()
    {
        string resPath = string.Format(UI_PREFAB_PATH_FORMAT, typeof(TView).Name);
        GameObject asset = Resources.Load<GameObject>(resPath);
        GameObject inst = GameObject.Instantiate(asset);
        TView view = new TView();
        view.SetOwner(inst);
        return view;
    }

    public override IEnumerator LoadViewAsync<TView>(Action<TView> callback)
    {
        string resPath = string.Format(UI_PREFAB_PATH_FORMAT, typeof(TView).Name);
        ResourceRequest request = Resources.LoadAsync<GameObject>(resPath);
        yield return request;
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        TView view = new TView();
        view.SetOwner(inst);
        callback?.Invoke(view);
    }

    public override TWindow LoadWindow<TWindow>(IUILayer uiLayer)
    {
        TWindow window = LoadView<TWindow>();
        window.UILayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        window.Create();
        return window;
    }

    public override IEnumerator LoadWindowAsync<TWindow>(IUILayer uiLayer, Action<TWindow> callback)
    {
        yield return LoadViewAsync<TWindow>(window =>
        {
            window.UILayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
            window.Create();
            callback?.Invoke(window);
        });
    } 
}