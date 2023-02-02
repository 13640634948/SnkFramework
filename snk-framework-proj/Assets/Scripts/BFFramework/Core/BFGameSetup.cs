using BFFramework.Runtime.UserInterface;
using BFFramework.Runtime.UserInterface.Layers;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace BFFramework.Runtime.Core
{
    public abstract class BFGameSetup<TSnkApplication> : UnitySetup<TSnkApplication>
        where TSnkApplication : BFClientApp
    {
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

        protected override ISnkViewLoader CreateViewLoader()
            => new BFViewLoader();


        protected override ISnkViewDispatcher CreateViewDispatcher()
            => new BFViewDispatcher();
    }
}