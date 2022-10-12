using System;
using System.Collections;
using System.Collections.Generic;
using SampleDevelop.Mvvm.Implments.UGUI;
using SampleDevelop.Test;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class CoroutineExecutor : MonoBehaviour, ICoroutineExecutor
{
    public IAsyncResult RunOnCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
        return default;
    }
}
public class UGUIWindowManager : WindowManagerBase
{
    public Camera mViewCamera { get; }

    private Dictionary<LAYER, ISnkUILayer> _layerDict;
    public GameObject mOwner;

    private ICoroutineExecutor _coroutineExecutor;
    private ISnkTransitionExecutor _transitionExecutor;

    private IResourceUILocator locator;

    public UGUIWindowManager()
    {
        //locator = new ResourceUILocator();
        GameObject goCoroutineExecutor = new GameObject();
        _coroutineExecutor = goCoroutineExecutor.AddComponent<CoroutineExecutor>();
        _transitionExecutor = new SnkTransitionPopupExecutor(_coroutineExecutor);

        this._layerDict = new Dictionary<LAYER, ISnkUILayer>();
        GameObject asset = Resources.Load<GameObject>("WindowRoot");
        GameObject inst = GameObject.Instantiate(asset);
        GameObject.DontDestroyOnLoad(inst);
        mViewCamera = inst.transform.Find("ViewCamera").GetComponent<Camera>();
        this.mOwner = inst;

        int layerCount = (int) LAYER.COUNT;
        for (int i = 0; i < layerCount; i++)
            this.CreateLayer((LAYER)i);
    }

    public override ISnkUILayer GetLayer(string layerName)
    {
        if (System.Enum.TryParse(layerName, out LAYER layer) == false)
            return null;

        if (this._layerDict.TryGetValue(layer, out ISnkUILayer layerTarget) == false)
            return null;
        return layerTarget;
    }

    public ISnkUILayer CreateLayer(LAYER layer)
    {
        if (this._layerDict.TryGetValue(layer, out var tmpLayer))
            return tmpLayer;
        
        string layerName = System.Enum.GetName(typeof(LAYER), layer);
        GameObject asset = Resources.Load<GameObject>("UILayer");
        GameObject inst = GameObject.Instantiate(asset);
        inst.name = layerName;
        if (inst.TryGetComponent<Canvas>(out var canvas) == false)
            canvas = inst.AddComponent<Canvas>();
        inst.transform.SetParent(this.mOwner.transform);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = this.mViewCamera;
        canvas.sortingLayerID = SortingLayer.NameToID(layerName);
        
        UGUILayer uguiLayer = new UGUILayer(this.locator);
        uguiLayer.mTransitionExecutor = _transitionExecutor;
        uguiLayer.SetOwner(canvas);
        this._layerDict.Add(layer, uguiLayer);
        return uguiLayer;
    }

    public TWindow OpenWindow<TWindow>(bool ignoreAnimation = false) where TWindow : class, ISnkWindow, new()
    {
        var layer = SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        var window = new TWindow();
        layer.Add(window);
        window.LoadViewOwner();
        window.Create();
        window.Show(ignoreAnimation);
        return window;
    }
    public IEnumerator OpenWindowAsync<TWindow>(Action<TWindow> callback, bool ignoreAnimation = false) where TWindow : class, ISnkWindow, new()
    {
        var layer = SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER), LAYER.normal));
        var window = new TWindow();
        layer.Add(window);
        yield return window.LoadViewOwnerAsync();
        window.Create();
        window.Show(ignoreAnimation);
        callback?.Invoke(window);
    }
}