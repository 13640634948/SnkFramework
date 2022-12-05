using SnkFramework.Mvvm.Demo.Implements.Layers;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using UnityEngine;

namespace SnkFramework.Mvvm.Demo
{
    namespace Implements
    {
        public class DemoMvvmInstaller : SnkMvvmInstaller
        {
            protected override ISnkViewCamera CreateViewCamera()
            {
                GameObject viewCameraGameObject = new GameObject(nameof(SnkViewCamera));
                viewCameraGameObject.hideFlags = HideFlags.HideInHierarchy;
                GameObject.DontDestroyOnLoad(viewCameraGameObject);
                return viewCameraGameObject.AddComponent<SnkViewCamera>();
            }

            protected override ISnkLayerContainer CreateLayerContainer()
            {
                GameObject layerContainerGameObject = new GameObject(nameof(SnkLayerContainer));
                GameObject.DontDestroyOnLoad(layerContainerGameObject);
                return layerContainerGameObject.AddComponent<SnkLayerContainer>();
            }

            protected override void RegiestLayer(ISnkLayerContainer container)
            {
                container.RegiestLayer<DemoUGUINormalLayer>();
                container.RegiestLayer<DemoUGUIDialogueLayer>();
                container.RegiestLayer<DemoUGUIGuideLayer>();
                container.RegiestLayer<DemoUGUITopLayer>();
                container.RegiestLayer<DemoUGUILoadingLayer>();
                container.RegiestLayer<DemoUGUISystemLayer>();
            }

            protected override ISnkViewFinder CreateViewFinder()
                => new DemoViewFinder();

            protected override ISnkViewLoader CreateViewLoader(ISnkViewFinder finder)
                => new DemoViewLoader(finder);

            protected override ISnkViewPresenter CreateViewPresenter(ISnkViewFinder finder, ISnkViewLoader loader,
                ISnkLayerContainer container)
                => new DemoPresenter(finder, loader, container);
        }
    }
}