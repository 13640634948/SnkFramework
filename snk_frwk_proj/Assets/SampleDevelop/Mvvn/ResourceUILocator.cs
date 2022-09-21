using System;
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
        //UIBehaviour uiBeh = inst.GetComponent<UIBehaviour>();
        //Debug.Log("inst:" + inst);
        //Debug.Log("uiBeh:" + uiBeh);
        TView view = new TView();
        view.SetOwner(inst);
        return view;
    }

    public override UILoadResult<TView> LoadViewAsync<TView>()
    {
        UILoadResult<TView> result = new UILoadResult<TView>();
        result.mResult = LoadView<TView>();;
        return result;
    }

    public override TWindow LoadWindow<TWindow>(IUILayer uiLayer)
    {
        TWindow window = LoadView<TWindow>();
        window.UILayer = uiLayer ?? SnkMvvmSetup.mWindowManager.GetLayer(Enum.GetName(typeof(LAYER),LAYER.normal));
        window.Create();
        return window;
    }

    public override UILoadResult<TWindow> LoadWindowAsync<TWindow>(IUILayer uiLayer)
        => LoadViewAsync<TWindow>();
}