using System;
using System.Threading.Tasks;

using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

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
public class SnkUGUINormalLayer : SnkUILayer
{
}

public class SnkUGUIDialogueLayer : SnkUILayer
{
}

public class SnkUGUIGuideLayer : SnkUILayer
{
}

public class SnkUGUITopLayer : SnkUILayer
{
}

public class SnkUGUILoadingLayer : SnkUILayer
{
}

public class SnkUGUISystemLayer : SnkUILayer
{
}

public class DemoPresenter : SnkViewPresenter
{
    public DemoPresenter(ISnkViewFinder viewFinder, ISnkViewLoader viewLoader, ISnkLayerContainer layerContainer) : base(viewFinder, viewLoader, layerContainer)
    {
    }

    public override SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
    {
        var attribute = base.CreatePresentationAttribute(viewModelType, viewType);
        if (attribute is SnkPresentationWindowAttribute windowAttribute)
        {
            windowAttribute.LayerType = typeof(SnkUGUINormalLayer);
        }

        return attribute;
    }
}

public class MvvmDemo : MonoBehaviour
{
    private SnkMvvmService _mvvmService;

    private void Awake()
    {
        GameObject viewCameraGameObject = new GameObject(nameof(SnkViewCamera));
        viewCameraGameObject.hideFlags = HideFlags.HideInHierarchy;
        ISnkViewCamera viewCamera = viewCameraGameObject.AddComponent<SnkViewCamera>();
        GameObject.DontDestroyOnLoad(viewCameraGameObject);

        GameObject layerContainerGameObject = new GameObject(nameof(SnkLayerContainer));
        GameObject.DontDestroyOnLoad(layerContainerGameObject);
        layerContainerGameObject.AddComponent<StandaloneInputModule>();
        SnkLayerContainer layerContainer = layerContainerGameObject.AddComponent<SnkLayerContainer>();
        layerContainer.RegiestLayer<SnkUGUINormalLayer>();
        layerContainer.RegiestLayer<SnkUGUIDialogueLayer>();
        layerContainer.RegiestLayer<SnkUGUIGuideLayer>();
        layerContainer.RegiestLayer<SnkUGUITopLayer>();
        layerContainer.RegiestLayer<SnkUGUILoadingLayer>();
        layerContainer.RegiestLayer<SnkUGUISystemLayer>();
        layerContainer.Build(viewCamera);


        ISnkViewFinder viewFinder = new SnkViewFinder();
        ISnkViewLoader viewLoader = new SnkViewLoader(viewFinder);
        ISnkViewPresenter presenter = new DemoPresenter(viewFinder, viewLoader, layerContainer);
        ISnkViewDispatcher dispatcher = new SnkViewDispatcher(presenter);

        ISnkViewModelCreator viewModelCreator = new SnkViewModelCreator();
        ISnkViewModelLocator viewModelLocator = new SnkViewModelLocator(viewModelCreator);
        ISnkViewModelLoader viewModelLoader = new SnkViewModelLoader(viewModelLocator);

        _mvvmService = new SnkMvvmService(dispatcher, viewModelLoader);
    }


    private TestViewModel viewModel;

    // Start is called before the first frame update
    async void Start()
    {
        viewModel = await _mvvmService.OpenWindow<TestViewModel>();
        Debug.Log("OpenWindow Finish");

        if (viewModel == null)
        {
            Debug.LogError("TestViewModel is null");
        }
        else
        {
            Debug.LogError(viewModel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _mvvmService.CloseWindow(viewModel);
        }
    }
}