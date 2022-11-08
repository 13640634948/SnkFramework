using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using UnityEngine;

public class SnkViewLoader : ISnkViewLoader
{
    private ISnkViewFinder _viewFinder;

    public SnkViewLoader(ISnkViewFinder viewFinder)
    {
        _viewFinder = viewFinder;
    }

    public async Task<SnkWindow> CreateView(SnkViewModelRequest request)
    {
        var viewType = _viewFinder.GetViewType(request.ViewModelType);
        return await CreateView(viewType);
    }

    public async Task<SnkWindow> CreateView(Type viewType)
    {
        var asset = await Resources.LoadAsync<GameObject>(viewType.Name);
        GameObject inst = UnityEngine.Object.Instantiate(asset) as GameObject;
        if (inst == null)
            return null;
        inst.name = viewType.Name;
        return inst.AddComponent(viewType) as SnkWindow;
    }

    public bool UnloadView(SnkWindow window)
    {
        UnityEngine.Object.Destroy(window.gameObject);
        return true;
    }
}