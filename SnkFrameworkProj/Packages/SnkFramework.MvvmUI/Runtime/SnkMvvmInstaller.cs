using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    public abstract class SnkMvvmInstaller
    {
        protected abstract ISnkViewCamera CreateViewCamera();
        protected abstract ISnkLayerContainer CreateLayerContainer();
        protected abstract void RegiestLayer(ISnkLayerContainer container);
        protected abstract ISnkViewFinder CreateViewFinder();
        protected abstract ISnkViewLoader CreateViewLoader(ISnkViewFinder finder);
        protected abstract ISnkViewPresenter CreateViewPresenter(ISnkViewFinder finder, ISnkViewLoader loader, ISnkLayerContainer container);

        //protected virtual IMvvmSettings GetMvvmSettings() => default;

        protected virtual ISnkViewDispatcher CreateViewDispatcher(ISnkViewPresenter presenter)
            => new SnkViewDispatcher(presenter);

        protected virtual ISnkViewModelCreator CreateViewModelCreator()
            => new SnkViewModelCreator();

        protected virtual ISnkViewModelLocator CreateViewModelLocator(ISnkViewModelCreator viewModelCreator)
            => new SnkViewModelLocator(viewModelCreator);

        protected virtual ISnkViewModelLoader CreateViewModelLoader(ISnkViewModelLocator viewModelLocator)
            => new SnkViewModelLoader(viewModelLocator);

        protected virtual ISnkMvvmService CreateMvvmService(ISnkViewDispatcher dispatcher, ISnkViewModelLoader viewModelLoader)
            => new SnkMvvmService(dispatcher, viewModelLoader);

        protected ISnkLayerContainer InitializeLayerContainer(ISnkViewCamera viewCamera)
        {
            var layerContainer = CreateLayerContainer();
            RegiestLayer(layerContainer);
            layerContainer.Build(viewCamera);
            return layerContainer;
        }

        protected ISnkViewDispatcher InitializeViewDispatcher(ISnkLayerContainer container)
        {
            var viewFinder = CreateViewFinder();
            var viewLoader = CreateViewLoader(viewFinder);
            var viewPresenter = CreateViewPresenter(viewFinder, viewLoader, container);
            return CreateViewDispatcher(viewPresenter);
        }

        protected ISnkViewModelLoader InitializeViewModelLoader()
        {
            var viewModelCreator = this.CreateViewModelCreator();
            var viewModelLocator = this.CreateViewModelLocator(viewModelCreator);
            return this.CreateViewModelLoader(viewModelLocator);
        }

        public ISnkMvvmService Install()
        {
            var viewCamera = CreateViewCamera();
            var layerContainer = InitializeLayerContainer(viewCamera);
            var viewDispatcher = InitializeViewDispatcher(layerContainer);
            var viewModelLoader = InitializeViewModelLoader();
            return this.CreateMvvmService(viewDispatcher, viewModelLoader);
        }
    }
}