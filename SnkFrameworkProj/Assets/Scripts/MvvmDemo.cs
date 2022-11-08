using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Mvvm.Runtime.Layer;
using UnityEngine;
using UnityEngine.EventSystems;

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
        var layerContainer = layerContainerGameObject.AddComponent<SnkLayerContainer>();
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