using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Demo.Implements;
using SnkFramework.Mvvm.Demo.Implements.Layers;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Runtime;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace DefaultNamespace
{
    public class DemoViewDispatcher : SnkViewDispatcher
    {
        private ISnkMainThreadAsyncDispatcher _mainThreadDispatcher=> Snk.IoCProvider.Resolve<ISnkMainThreadAsyncDispatcher>();
        
        public override Task ExecuteOnUIThreadAsync(Func<Task> action, bool maskExceptions = true)
            => _mainThreadDispatcher?.ExecuteOnMainThreadAsync(action, maskExceptions);
    }

    public class DemoSetup : UnitySetup<DemoApp>
    {
        protected override void RegisterLayer(ISnkLayerContainer container)
        {
            container.RegiestLayer<DemoUGUINormalLayer>();
            container.RegiestLayer<DemoUGUIDialogueLayer>();
            container.RegiestLayer<DemoUGUIGuideLayer>();
            container.RegiestLayer<DemoUGUITopLayer>();
            container.RegiestLayer<DemoUGUILoadingLayer>();
            container.RegiestLayer<DemoUGUISystemLayer>();
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
            => new DemoViewLoader();

        protected override ISnkViewPresenter CreateViewPresenter()
            => new DemoPresenter();

        protected override ISnkViewDispatcher CreateViewDispatcher()
            => new DemoViewDispatcher();
    }
}