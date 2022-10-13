using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Core.Log;

namespace SnkFramework.Mvvm.Core
{
    public class SnkMvvmSetup
    {
        static public System.Func<ISnkMvvmSettings> SettingCreator;
        static public System.Func<ISnkMvvmManager> ManagerCreator;
        static public System.Func<ISnkMvvmLogger> LoggerCreator;
        static public System.Func<ISnkMvvmCoroutineExecutor> CoroutineExecutorCreator;
        static public System.Func<ISnkMvvmLoader> LoaderCreator;

        static public void Initialize()
        {
            SnkBindingSetup.Initialize();
            
            SnkIoCProvider iocProvider = SnkIoCProvider.Instance;
            iocProvider.Register(SettingCreator?.Invoke());
            iocProvider.Register(ManagerCreator?.Invoke());
            iocProvider.Register(LoggerCreator?.Invoke());
            iocProvider.Register(CoroutineExecutorCreator?.Invoke());
            iocProvider.Register(LoaderCreator?.Invoke());
        }

        static public void Uninitialize()
        {
            SnkIoCProvider iocProvider = SnkIoCProvider.Instance;
            iocProvider.Unregister<ISnkMvvmSettings>();
            iocProvider.Unregister<ISnkMvvmManager>();
            iocProvider.Unregister<ISnkMvvmLogger>();
            iocProvider.Unregister<ISnkMvvmCoroutineExecutor>();
            iocProvider.Unregister<ISnkMvvmLoader>();
            
            SnkBindingSetup.Uninitialize();
        }
    }

}