using System;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Logging;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public partial class SnkSetup
        {
            //protected abstract ISnkLoggerProvider CreateLoggerProvider();
            protected abstract ISnkApplication CreateApp(ISnkIoCProvider iocProvider);
            //protected abstract ISnkViewCamera CreateViewCamera();
            protected abstract void RegisterLayer(ISnkLayerContainer container);
            //protected abstract ISnkLayerContainer CreateLayerContainer();
            protected virtual ISnkViewPresenter CreateViewPresenter() => new SnkViewPresenter();
            
            protected void InitializeLogService(ISnkIoCProvider iocProvider)
            {
                var unityLogFactory = this.CreateLoggerFactory();
                SnkLogHost.Registry(unityLogFactory);
                logger = SnkLogHost.GetLogger(this.GetType());
                iocProvider.RegisterSingleton(unityLogFactory);
            }
            
            protected abstract ISnkLogFactory CreateLoggerFactory();
            
            /// <summary>
            /// 初始化层级管理器
            /// </summary>
            /// <param name="viewCamera"></param>
            /// <returns></returns>
            protected abstract void InitializeLayerContainer(ISnkIoCProvider iocProvider);
            
            protected virtual ISnkViewModelLoader CreateViewModelLoader() 
                => new SnkViewModelLoader();

            protected virtual void InitializeViewModelLoader(ISnkIoCProvider iocProvider)
                => iocProvider.RegisterSingleton(CreateViewModelLoader());

            protected virtual void RegisterDefaultDependencies(ISnkIoCProvider iocProvider)
            {
                iocProvider.LazyConstructAndRegisterSingleton<ISnkSettings, SnkSettings>();
                iocProvider.RegisterSingleton<ISnkPluginManager>(() => new SnkPluginManager(GetPluginConfiguration));
                iocProvider.RegisterSingleton(CreateApp(iocProvider));
                iocProvider.RegisterSingleton(CreateViewPresenter());
                iocProvider.LazyConstructAndRegisterSingleton<ISnkMvvmService, ISnkViewDispatcher, ISnkViewModelLoader>(
                    (dispatcher,viewModelLoader)=> new SnkMvvmService(dispatcher,viewModelLoader));
                iocProvider.RegisterSingleton(() => new SnkViewModelByNameLookup());
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelByNameLookup, SnkViewModelByNameLookup>(lookup=>lookup);
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelByNameRegistry, SnkViewModelByNameLookup>(lookup=>lookup);
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelTypeFinder, SnkViewModelViewTypeFinder>();
                iocProvider.LazyConstructAndRegisterSingleton<ISnkTypeToTypeLookupBuilder, SnkViewModelViewLookupBuilder>();
            }

            protected abstract ISnkViewsContainer CreateViewContainer();
            
            protected virtual void InitializeViewContainer(ISnkIoCProvider iocProvider)
                => iocProvider.RegisterSingleton(CreateViewContainer());    

            protected virtual ISnkIocOptions CreateIocOptions()
                => new SnkIocOptions();

            protected virtual ISnkIoCProvider CreateIocProvider()
                => SnkIoCProvider.Initialize(CreateIocOptions());

            protected virtual ISnkIoCProvider InitializeIoC()
            {
                // initialize the IoC registry, then add it to it self
                var iocProvider = CreateIocProvider();
                iocProvider.RegisterSingleton(iocProvider);
                iocProvider.RegisterSingleton<ISnkSetup>(this);
                return iocProvider;
            }

            protected virtual ISnkSettings InitializeSettings(ISnkIoCProvider iocProvider)
                => CreateSettings(iocProvider);

            protected virtual ISnkSettings CreateSettings(ISnkIoCProvider iocProvider)
                =>iocProvider.Resolve<ISnkSettings>();

            protected virtual void InitializeSingletonCache()
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                //SnkSingletonCache.Initialize();
#pragma warning restore CA2000 // Dispose objects before losing scope
            }

            protected virtual ISnkMainThread CreateMainThreadSynchronizationContext()
                => new SnkMainThread();

            protected virtual void InitializeDispatcher(ISnkIoCProvider iocProvider)
            {
                var dispatcher = new SnkMainThreadAsyncDispatcher();
                iocProvider.RegisterSingleton<ISnkMainThreadAsyncDispatcher>(dispatcher);
                iocProvider.RegisterSingleton<ISnkMainThreadDispatcher>(dispatcher);
            }
            
            /// <summary>
            /// 初始化各个平台
            /// </summary>
            protected virtual void InitializeBasePlatform(ISnkIoCProvider iocProvider)
            {
                var mainThread = CreateMainThreadSynchronizationContext();
                iocProvider.RegisterSingleton(mainThread);
            }

            protected abstract ISnkViewDispatcher CreateViewDispatcher();


            protected virtual void InitializeViewDispatcher(ISnkIoCProvider iocProvider)
            {
                var dispatcher = CreateViewDispatcher();
                iocProvider.RegisterSingleton(dispatcher);
            }
            
            public void InitializePrimary()
            {
                if (State != eSnkSetupState.Uninitialized)
                {
                    logger?.Error($"InitializePrimary() called when State is not Uninitialized. State: {State}");
                    return;
                }

                try
                {
                    State = eSnkSetupState.InitializingPrimary;
                    this._iocProvider = InitializeIoC();

                    this.InitializeLogService(this._iocProvider);
                    logger?.Info("Setup: Register Default Dependencies");
                    this.RegisterDefaultDependencies(this._iocProvider);
                    logger?.Info("Setup: Primary start");
                    this.InitializeFirstChance(_iocProvider);
                    this.InitializeSettings(_iocProvider);
                    this.InitializeBasePlatform(_iocProvider);

                    this.InitializeDispatcher(_iocProvider);
                    this.InitializeViewDispatcher(_iocProvider);
                    
                    this.InitializeLayerContainer(_iocProvider);

                    State = eSnkSetupState.InitializedPrimary;
                }
                catch (Exception e)
                {
                    logger?.Error("InitializePrimary() Failed initializing primary dependencies", e);
                    throw;
                }
            }
        }
    }
}