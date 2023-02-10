using BFFramework.Runtime.Managers;
using BFFramework.Runtime.UserInterface;
using BFFramework.Runtime.UserInterface.Layers;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace BFFramework.Runtime.Core
{
    public abstract class BFGameSetup<TSnkApplication> : SnkUnitySetup<TSnkApplication>
        where TSnkApplication : BFClientApp
    {
        protected override string UserInterfaceAssemblyName => "Game.Contexts.UserInterfaces";

        protected override void RegisterDefaultDependencies(ISnkIoCProvider iocProvider)
        {
            base.RegisterDefaultDependencies(iocProvider);
            BFManagerHub.Initialize(iocProvider);
        }

        protected override void RegisterLayer(ISnkLayerContainer container)
        {
            container.RegiestLayer<BFUGUINormalLayer>(true);
            container.RegiestLayer<BFUGUIDialogueLayer>();
            container.RegiestLayer<BFUGUIGuideLayer>();
            container.RegiestLayer<BFUGUITopLayer>();
            container.RegiestLayer<BFUGUILoadingLayer>();
            container.RegiestLayer<BFUGUISystemLayer>();
        }

        protected virtual ISnkLayerContainer CreateLayerContainer()
        {
            var layerContainerGameObject = new GameObject(nameof(SnkLayerContainer));
            GameObject.DontDestroyOnLoad(layerContainerGameObject);
            return layerContainerGameObject.AddComponent<SnkLayerContainer>();
        }

        protected IBFCameraManager ResolveCameraManager(ISnkIoCProvider iocProvider)
            => iocProvider.Resolve<IBFCameraManager>();

        protected override void InitializeLayerContainer(ISnkIoCProvider iocProvider)
        {
            var layerContainer = CreateLayerContainer();
            this.RegisterLayer(layerContainer);
            var cameraMgr = ResolveCameraManager(iocProvider);
            layerContainer.Build(cameraMgr.orthographicCamera);
            iocProvider.RegisterSingleton(layerContainer);
        }

        protected override ISnkViewsContainer CreateViewContainer()
            => new BFViewsContainer();

        protected override ISnkViewDispatcher CreateViewDispatcher()
            => new BFViewDispatcher();

        protected override ISnkNameMapping CreateViewToViewModelNaming()
            => new SnkPostfixAwareViewToViewModelNameMapping("View", "Window");
    }
}