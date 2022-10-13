using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Core.Log;

namespace SnkFramework.Mvvm.Core
{
    public class SnkMvvmSetup
    {
        static public ISnkMvvmSettings mSettings;
        static public ISnkMvvmManager MSnkMvvmManager;
        static public ISnkMvvmLogger MSnkMvvmLogger;
        static public ISnkMvvmCoroutineExecutor mCoroutineExecutor;
        static public ISnkMvvmLoader mLoader;

        static public void Initialize(
            ISnkMvvmManager snkMvvmManager,
            ISnkMvvmCoroutineExecutor coroutineExecutor,
            ISnkMvvmLoader loader,
            ISnkMvvmLogger snkMvvmLogger = null,
            ISnkMvvmSettings settings = null
            )
        {
            SnkBindingSetup.Initialize();
            
            mSettings = settings;
            MSnkMvvmLogger = snkMvvmLogger;
            mLoader = loader;
            MSnkMvvmManager = snkMvvmManager;
            mCoroutineExecutor = coroutineExecutor;
        }
    }

}