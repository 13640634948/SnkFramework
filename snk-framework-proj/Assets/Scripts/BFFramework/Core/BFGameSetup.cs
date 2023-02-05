using System.Reflection;
using BFFramework.Runtime.UserInterface;
using BFFramework.Runtime.UserInterface.Layers;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace BFFramework.Runtime.Core
{
    public abstract class BFGameSetup<TSnkApplication> : UnitySetup<TSnkApplication>
        where TSnkApplication : BFClientApp
    {
        protected override string UserInterfaceAssemblyName => "Game.Contexts.UserInterfaces";

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

        protected override ISnkViewCamera CreateViewCamera()
        {
            var viewCameraGameObject = new GameObject(nameof(SnkViewCamera));
            GameObject.DontDestroyOnLoad(viewCameraGameObject);
            return viewCameraGameObject.AddComponent<SnkViewCamera>();
        }

        protected override SnkViewLoader CreateViewLoader()
            => new BFViewLoader();

        protected override ISnkViewDispatcher CreateViewDispatcher()
            => new BFViewDispatcher();

        protected override IMvxNameMapping CreateViewToViewModelNaming()
            => new MvxPostfixAwareViewToViewModelNameMapping("View", "Window");
    }
}