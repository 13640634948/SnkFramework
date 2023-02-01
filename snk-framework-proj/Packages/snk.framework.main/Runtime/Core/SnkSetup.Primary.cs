using System;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public partial class SnkSetup
        {
            protected abstract ISnkLoggerProvider CreateLoggerProvider();

            protected abstract IMvxApplication CreateApp(ISnkIoCProvider iocProvider);

            protected virtual ISnkLoggerFactory CreateLoggerFactory()
                => new SnkLoggerFactory();

            protected void InitializeLogService(ISnkIoCProvider iocProvider)
            {
                var loggerProvider = this.CreateLoggerProvider();
                //Snk.IoCProvider.RegisterSingleton<ISnkLoggerProvider>(loggerProvider);

                var loggerFactory = this.CreateLoggerFactory();
                //Snk.IoCProvider.RegisterSingleton<ISnkLoggerFactory>(loggerFactory);

                loggerFactory.AddLoggerProvider(loggerProvider);
                logger = loggerFactory.CreateLogger<SnkSetup>();
            }

            //protected abstract ISnkCompressor CreateCompressor();
            //protected abstract ISnkJsonParser CreateJsonParser();
            //protected virtual ISnkCodeGenerator CreateCodeGenerator() => new SnkMD5Generator();

            //protected abstract ISnkMvvmInstaller CreateMvvmInstaller();

            protected abstract void RegisterLayer(ISnkLayerContainer container);
            protected abstract ISnkLayerContainer CreateLayerContainer();

            /// <summary>
            /// 初始化层级管理器
            /// </summary>
            /// <param name="viewCamera"></param>
            /// <returns></returns>
            protected ISnkLayerContainer InitializeLayerContainer(ISnkViewCamera viewCamera)
            {
                var layerContainer = CreateLayerContainer();
                RegisterLayer(layerContainer);
                layerContainer.Build(viewCamera);
                return layerContainer;
            }
            
            protected virtual ISnkMvvmService CreateMvvmService(ISnkIoCProvider iocProvider)
            {
                //return CreateMvvmInstaller().Install(iocProvider);
                return iocProvider.Resolve<ISnkMvvmService>();
            }
            protected abstract ISnkViewCamera CreateViewCamera();
            protected abstract ISnkViewFinder CreateViewFinder(ISnkIoCProvider iocProvider);
            protected abstract ISnkViewLoader CreateViewLoader(ISnkViewFinder finder);

            
            
            protected virtual void InitializeMvvmService(ISnkIoCProvider iocProvider)
            {
                var dispatch = iocProvider.Resolve<ISnkMainThreadAsyncDispatcher>();
                var mvvmService = CreateMvvmService(iocProvider);

                //init mvvm routes
                //return mvvmService;
            }

            protected virtual void RegisterDefaultDependencies(ISnkIoCProvider iocProvider)
            {
                iocProvider.LazyConstructAndRegisterSingleton<ISnkSettings, SnkSettings>();
                iocProvider.RegisterSingleton<ISnkPluginManager>(() => new MvxPluginManager(GetPluginConfiguration));
                iocProvider.RegisterSingleton(CreateApp(iocProvider));
                
                
                var viewCamera = CreateViewCamera();
                var layerContainer = InitializeLayerContainer(viewCamera);
                iocProvider.RegisterSingleton(layerContainer);

                var viewFinder = CreateViewFinder(iocProvider);
                iocProvider.RegisterSingleton(viewFinder);
                var viewLoader = CreateViewLoader(viewFinder);
                iocProvider.RegisterSingleton(viewLoader);
                
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelCreator, SnkViewModelCreator>();
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelLocator, ISnkViewModelCreator>(
                    (a) => new SnkViewModelLocator(a));
                iocProvider.LazyConstructAndRegisterSingleton<ISnkViewModelLoader, ISnkViewModelLocator>(
                    (a) => new SnkViewModelLoader(a));
                iocProvider.LazyConstructAndRegisterSingleton<ISnkMvvmService, ISnkViewDispatcher, ISnkViewModelLoader>(
                    (a,b)=> new SnkMvvmService(a,b));
                //ioCProvider.RegisterSingleton<ISnkJsonParser>(CreateJsonParser());
                //ioCProvider.RegisterSingleton<ISnkCompressor>(CreateCompressor());
                //ioCProvider.RegisterSingleton<ISnkCodeGenerator>(CreateCodeGenerator());
            }

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
            {
                var settings = CreateSettings(iocProvider);
                return settings;
            }

            protected virtual ISnkSettings CreateSettings(ISnkIoCProvider iocProvider)
                =>iocProvider.Resolve<ISnkSettings>();

            protected virtual void InitializeSingletonCache()
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                //SnkSingletonCache.Initialize();
#pragma warning restore CA2000 // Dispose objects before losing scope
            }

            protected virtual void InitializeDispatcher(ISnkIoCProvider iocProvider)
            {
                //主线程派发器
            }
            
            /// <summary>
            /// 初始化各个平台
            /// </summary>
            protected virtual void InitializeBasePlatform(ISnkIoCProvider iocProvider)
            {
            
            }

            protected virtual ISnkViewDispatcher CreateViewDispatcher(ISnkViewPresenter presenter)
            {
                var dispatcher = new SnkViewDispatcher(presenter);
                return dispatcher;
            }

            protected abstract ISnkViewPresenter CreateViewPresenter();

            protected ISnkViewPresenter _presenter;

            protected ISnkViewPresenter Presenter
            {
                get
                {
                    if(_presenter == null)
                        _presenter = CreateViewPresenter();
                    return _presenter;
                }
            }

            protected virtual void InitializeViewDispatcher(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);

                var dispatcher = CreateViewDispatcher(Presenter);
                iocProvider.RegisterSingleton(dispatcher);
                iocProvider.RegisterSingleton<ISnkMainThreadAsyncDispatcher>(dispatcher);
                iocProvider.RegisterSingleton<ISnkMainThreadDispatcher>(dispatcher);
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
                    this.RegisterDefaultDependencies(this._iocProvider);
                    logger?.Info("Setup: Primary start");
                    this.InitializeFirstChance(_iocProvider);
                    this.InitializeSettings(_iocProvider);
                    this.InitializeBasePlatform(_iocProvider);
                    //InitializeSingletonCache();
                    this.InitializeViewDispatcher(_iocProvider);
                    this.InitializeDispatcher(_iocProvider);
                    State = eSnkSetupState.InitializedPrimary;
                }
                catch (Exception e)
                {
                    logger?.Exception(e, "InitializePrimary() Failed initializing primary dependencies");
                    throw;
                }
            }
        }
    }
}