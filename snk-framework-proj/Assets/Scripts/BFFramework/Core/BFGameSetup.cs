using BFFramework.Runtime.Managers;
using BFFramework.Runtime.UserInterface;
using BFFramework.Runtime.UserInterface.Layers;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Runtime;
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
            InitializeManagers(iocProvider);
        }

        protected virtual void InitializeManagers(ISnkIoCProvider iocProvider)
        {
            RegisterManager<IBFModuleManager, BFModuleManager>(iocProvider);
            RegisterManager<ICameraManager, CameraManager>(iocProvider);
        }

        protected void RegisterManager<TManager, TMgrInstance>(ISnkIoCProvider iocProvider)
            where TMgrInstance : class, IBFManager
        {
            SnkLogHost.Default?.Info("Setup: Register " + typeof(TMgrInstance));
            var service = iocProvider.IoCConstruct<TMgrInstance>();
            iocProvider.RegisterSingleton(typeof(TManager), service);
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

        protected override ISnkLayerContainer CreateLayerContainer()
        {
            var layerContainerGameObject = new GameObject(nameof(SnkLayerContainer));
            GameObject.DontDestroyOnLoad(layerContainerGameObject);
            return layerContainerGameObject.AddComponent<SnkLayerContainer>();
        }

        protected override ISnkViewsContainer CreateViewContainer()
            => new BFViewsContainer();

        protected override ISnkViewCamera CreateViewCamera()
        {
            var viewCameraGameObject = new GameObject(nameof(SnkViewCamera));
            GameObject.DontDestroyOnLoad(viewCameraGameObject);
            return viewCameraGameObject.AddComponent<SnkViewCamera>();
        }

        protected override ISnkViewDispatcher CreateViewDispatcher()
            => new BFViewDispatcher();

        protected override ISnkNameMapping CreateViewToViewModelNaming()
            => new SnkPostfixAwareViewToViewModelNameMapping("View", "Window");
    }
}