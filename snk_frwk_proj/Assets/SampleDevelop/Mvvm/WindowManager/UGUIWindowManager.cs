using System.Collections.Generic;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class UGUIWindowManager : WindowManagerBase
{
    public Camera mViewCamera { get; }

    private Dictionary<LAYER, IUILayer> _layerDict;
    public GameObject mOwner;
    public UGUIWindowManager()
    {
        this._layerDict = new Dictionary<LAYER, IUILayer>();
        GameObject asset = Resources.Load<GameObject>("WindowRoot");
        GameObject inst = GameObject.Instantiate(asset);
        GameObject.DontDestroyOnLoad(inst);
        mViewCamera = inst.transform.Find("ViewCamera").GetComponent<Camera>();
        this.mOwner = inst;

        int layerCount = (int) LAYER.COUNT;
        for (int i = 0; i < layerCount; i++)
            this.CreateLayer((LAYER)i);
    }

    public override IUILayer GetLayer(string layerName)
    {
        if (System.Enum.TryParse(layerName, out LAYER layer) == false)
            return null;

        if (this._layerDict.TryGetValue(layer, out IUILayer layerTarget) == false)
            return null;
        return layerTarget;
    }

    public IUILayer CreateLayer(LAYER layer)
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
        
        UGUILayer uguiLayer = new UGUILayer();
        uguiLayer.SetOwner(canvas);
        this._layerDict.Add(layer, uguiLayer);
        return uguiLayer;
    }

}