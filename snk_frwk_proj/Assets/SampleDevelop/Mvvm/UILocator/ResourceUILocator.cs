
/*
using System;
using System.Collections;
using Windows.LoginWindow;
using SampleDevelop.Test;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class ResourceUILocator : UILocator, IResourceUILocator
{
    protected readonly string UI_PREFAB_PATH_FORMAT = "UI/Prefabs/{0}";

    private string getViewPath<TView>()
        => string.Format(UI_PREFAB_PATH_FORMAT, typeof(TView).Name);

    public override TView LoadView<TView>()
    {
        string resPath = getViewPath<TView>();
        GameObject asset = Resources.Load<GameObject>(resPath);
        GameObject inst = GameObject.Instantiate(asset);
        TView view = new TView();
        view.Create();
        return view;
    }

    public override IEnumerator LoadViewAsync<TView>(Action<TView> callback)
    {
        string resPath = getViewPath<TView>();
        ResourceRequest request = Resources.LoadAsync<GameObject>(resPath);
        yield return request;
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        TView view = new TView();
        view.Create();
        callback?.Invoke(view);
    }

    public override TWindow LoadWindow<TWindow>(ISnkUILayer uiLayer)
    {
        uiLayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        TWindow window = new TWindow();
        uiLayer.Add(window);
        
        string resPath = getViewPath<TWindow>();
        GameObject asset = Resources.Load<GameObject>(resPath);
        GameObject inst = GameObject.Instantiate(asset);
        inst.name = "[S]" + window.mPriority + "="+ Time.frameCount;;
        window.Create();

        return window;
    }

    public override IEnumerator LoadWindowAsync<TWindow>(ISnkUILayer uiLayer, Action<TWindow> callback)
    {
        uiLayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        TWindow window = new TWindow();
        uiLayer.Add(window);
        window.Load();
        
        string resPath = getViewPath<TWindow>();
        ResourceRequest request = Resources.LoadAsync<GameObject>(resPath);
        yield return request;
        yield return new WaitForSeconds(1.0f);
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        inst.name = "[A]" + window.mPriority + "="+ Time.frameCount;
        
        window.Create();
        callback?.Invoke(window);
    }

}*/