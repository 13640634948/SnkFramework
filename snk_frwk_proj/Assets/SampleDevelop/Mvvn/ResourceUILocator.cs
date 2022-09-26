using System;
using System.Collections;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
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
        view.SetOwner(inst);
        return view;
    }

    public override IEnumerator LoadViewAsync<TView>(Action<TView> callback)
    {
        string resPath = getViewPath<TView>();
        ResourceRequest request = Resources.LoadAsync<GameObject>(resPath);
        yield return request;
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        TView view = new TView();
        view.SetOwner(inst);
        callback?.Invoke(view);
    }

    public override TWindow LoadWindow<TWindow>(IUILayer uiLayer)
    {
        uiLayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        int sortingOrder = uiLayer.AddSortingOrder();
        string resPath = getViewPath<TWindow>();
        GameObject asset = Resources.Load<GameObject>(resPath);
        GameObject inst = GameObject.Instantiate(asset);
        TWindow window = new TWindow();
        window.SetOwner(inst);
        window.UILayer = uiLayer;
        window.mSortingOrder = sortingOrder;
        window.Create();
        return window;
    }

    public override IEnumerator LoadWindowAsync<TWindow>(IUILayer uiLayer, Action<TWindow> callback)
    {
        uiLayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        int sortingOrder = uiLayer.AddSortingOrder();
        Debug.Log("LoadWindowAsync:" + sortingOrder);
        string resPath = getViewPath<TWindow>();
        ResourceRequest request = Resources.LoadAsync<GameObject>(resPath);
        yield return request;
        yield return new WaitForSeconds(1.0f);
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        TWindow window = new TWindow();
        window.SetOwner(inst);
        window.UILayer = uiLayer;
        window.mSortingOrder = sortingOrder;
        window.Create();
        callback?.Invoke(window);
    }

}