using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.Mvvm;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;

public class SnkViewFinder : ISnkViewFinder
{
    public Type GetViewType(Type viewModelType)
    {
        return typeof(TestWindow);
    }

    public Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel
        => GetViewType(typeof(TViewModel));
}

public class SnkViewLoader : ISnkViewLoader
{
    private ISnkViewFinder _viewFinder;
    public SnkViewLoader(ISnkViewFinder viewFinder)
    {
        _viewFinder = viewFinder;
    }

    public async Task<SnkUIBehaviour> CreateView(SnkViewModelRequest request)
    {
        var viewType = _viewFinder.GetViewType(request.ViewModelType);
        return await CreateView(viewType);
    }

    public async Task<SnkUIBehaviour> CreateView(Type viewType)
    {
        var asset = await Resources.LoadAsync<GameObject>(viewType.Name);
        GameObject inst = GameObject.Instantiate(asset) as GameObject;
        return inst.AddComponent(viewType) as SnkUIBehaviour;
    }
}

public class MvvmDemo : MonoBehaviour
{
    private SnkMvvmService _mvvmService;
    private void Awake()
    {
        _mvvmService = new SnkMvvmService();
        ISnkViewFinder viewFinder = new SnkViewFinder();
        ISnkViewLoader viewLoader = new SnkViewLoader(viewFinder);
        ISnkViewPresenter presenter = new SnkViewPresenter(viewFinder,viewLoader);
        _mvvmService._viewDispatcher = new SnkViewDispatcher(presenter);

        ISnkViewModelCreator viewModelCreator = new SnkViewModelCreator();
        ISnkViewModelLocator viewModelLocator = new SnkViewModelLocator(viewModelCreator);
        _mvvmService._viewModelLoader = new SnkViewModelLoader(viewModelLocator);
    }

    // Start is called before the first frame update
    async void Start()
    {
        TestViewModel viewModel = await _mvvmService.OpenWindow<TestViewModel>();
        Debug.LogError(viewModel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
