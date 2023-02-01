using System;
using SnkFramework.IoC;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Runtime.Core.Setup;

namespace SnkFramework
{
    namespace Core
    {
        public partial class SnkSetup
        {
            protected abstract ISnkLoggerProvider CreateLoggerProvider();

            protected virtual ISnkLoggerFactory CreateLoggerFactory()
                => new SnkLoggerFactory();

            protected void InitializeLogService(ISnkIoCProvider iocProvider)
            {
                var loggerProvider = this.CreateLoggerProvider();
                //Snk.IoCProvider.RegisterSingleton<ISnkLoggerProvider>(loggerProvider);

                var loggerFactory = this.CreateLoggerFactory();
                //Snk.IoCProvider.RegisterSingleton<ISnkLoggerFactory>(loggerFactory);

                loggerFactory.AddLoggerProvider(loggerProvider);
                //logger = loggerFactory.CreateLogger<SnkSetup>();
            }

            //protected abstract ISnkCompressor CreateCompressor();
            //protected abstract ISnkJsonParser CreateJsonParser();
            //protected virtual ISnkCodeGenerator CreateCodeGenerator() => new SnkMD5Generator();

            protected virtual void RegisterDefaultDependencies(ISnkIoCProvider ioCProvider)
            {
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
                    //this.InitializeViewDispatcher(_iocProvider);
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