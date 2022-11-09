using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    public abstract class SnkMvvmInstaller
    {
        /// <summary>
        /// 创建视图摄像机
        /// </summary>
        /// <returns></returns>
        protected abstract ISnkViewCamera CreateViewCamera();
        
        /// <summary>
        /// 创建层级容器
        /// </summary>
        /// <returns></returns>
        protected abstract ISnkLayerContainer CreateLayerContainer();
        
        /// <summary>
        /// 注册层级对象到层级容器中
        /// </summary>
        /// <param name="container"></param>
        protected abstract void RegiestLayer(ISnkLayerContainer container);
        
        /// <summary>
        /// 视图查找器
        /// </summary>
        /// <returns></returns>
        protected abstract ISnkViewFinder CreateViewFinder();
        
        /// <summary>
        /// 视图加载器
        /// </summary>
        /// <param name="finder"></param>
        /// <returns></returns>
        protected abstract ISnkViewLoader CreateViewLoader(ISnkViewFinder finder);
        
        /// <summary>
        /// 视图的调度管理器
        /// </summary>
        /// <param name="finder"></param>
        /// <param name="loader"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected abstract ISnkViewPresenter CreateViewPresenter(ISnkViewFinder finder, ISnkViewLoader loader, ISnkLayerContainer container);

        //protected virtual IMvvmSettings GetMvvmSettings() => default;

        /// <summary>
        /// 创建主线程调度器
        /// </summary>
        /// <param name="presenter"></param>
        /// <returns></returns>
        protected virtual ISnkViewDispatcher CreateViewDispatcher(ISnkViewPresenter presenter)
            => new SnkViewDispatcher(presenter);

        /// <summary>
        /// 创建视图模型创建者
        /// </summary>
        /// <returns></returns>
        protected virtual ISnkViewModelCreator CreateViewModelCreator()
            => new SnkViewModelCreator();

        /// <summary>
        /// 创建视图模型定位者
        /// </summary>
        /// <param name="viewModelCreator"></param>
        /// <returns></returns>
        protected virtual ISnkViewModelLocator CreateViewModelLocator(ISnkViewModelCreator viewModelCreator)
            => new SnkViewModelLocator(viewModelCreator);

        /// <summary>
        /// 创建视图模型加载器
        /// </summary>
        /// <param name="viewModelLocator"></param>
        /// <returns></returns>
        protected virtual ISnkViewModelLoader CreateViewModelLoader(ISnkViewModelLocator viewModelLocator)
            => new SnkViewModelLoader(viewModelLocator);

        /// <summary>
        /// 创建Mvvm服务
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="viewModelLoader"></param>
        /// <returns></returns>
        protected virtual ISnkMvvmService CreateMvvmService(ISnkViewDispatcher dispatcher, ISnkViewModelLoader viewModelLoader)
            => new SnkMvvmService(dispatcher, viewModelLoader);

        /// <summary>
        /// 初始化层级管理器
        /// </summary>
        /// <param name="viewCamera"></param>
        /// <returns></returns>
        protected ISnkLayerContainer InitializeLayerContainer(ISnkViewCamera viewCamera)
        {
            var layerContainer = CreateLayerContainer();
            RegiestLayer(layerContainer);
            layerContainer.Build(viewCamera);
            return layerContainer;
        }

        /// <summary>
        /// 初始化视图线程调度器
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        protected ISnkViewDispatcher InitializeViewDispatcher(ISnkLayerContainer container)
        {
            var viewFinder = CreateViewFinder();
            var viewLoader = CreateViewLoader(viewFinder);
            var viewPresenter = CreateViewPresenter(viewFinder, viewLoader, container);
            return CreateViewDispatcher(viewPresenter);
        }

        /// <summary>
        /// 初始化视图模型加载器
        /// </summary>
        /// <returns></returns>
        protected ISnkViewModelLoader InitializeViewModelLoader()
        {
            var viewModelCreator = this.CreateViewModelCreator();
            var viewModelLocator = this.CreateViewModelLocator(viewModelCreator);
            return this.CreateViewModelLoader(viewModelLocator);
        }

        /// <summary>
        /// 安装MvvmService
        /// </summary>
        /// <returns></returns>
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