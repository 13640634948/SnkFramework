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
using SnkFramework.Mvvm.Runtime.Layer;

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
        GameObject layerContainerGameObject = new GameObject(nameof(SnkLayerContainer));
        GameObject.DontDestroyOnLoad(layerContainerGameObject);
        layerContainerGameObject.AddComponent<RectTransform>();
        ISnkLayerContainer layerContainer = layerContainerGameObject.AddComponent<SnkLayerContainer>(); 
        layerContainer.RegiestLayer<SnkUGUINormalLayer>();
        layerContainer.RegiestLayer<SnkUGUIDialogueLayer>();
        layerContainer.RegiestLayer<SnkUGUIGuideLayer>();
        layerContainer.RegiestLayer<SnkUGUITopLayer>();
        layerContainer.RegiestLayer<SnkUGUILoadingLayer>();
        layerContainer.RegiestLayer<SnkUGUISystemLayer>();
        layerContainer.Build();
        
        
        ISnkViewFinder viewFinder = new SnkViewFinder();
        ISnkViewLoader viewLoader = new SnkViewLoader(viewFinder);
        ISnkViewPresenter presenter = new SnkViewPresenter(viewFinder,viewLoader,layerContainer);
        ISnkViewDispatcher dispatcher = new SnkViewDispatcher(presenter);

        ISnkViewModelCreator viewModelCreator = new SnkViewModelCreator();
        ISnkViewModelLocator viewModelLocator = new SnkViewModelLocator(viewModelCreator);
        ISnkViewModelLoader viewModelLoader = new SnkViewModelLoader(viewModelLocator);
        
        _mvvmService = new SnkMvvmService(dispatcher, viewModelLoader);
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
