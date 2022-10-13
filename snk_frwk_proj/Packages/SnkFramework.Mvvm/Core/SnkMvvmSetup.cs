using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Core.Log;

namespace SnkFramework.Mvvm.Core
{
    public class SnkMvvmSetup
    {
        static public ISnkMvvmSettings mSettings;
        static public IWindowManager mWindowManager;
        static public IMvvmLog mMvvmLog;
        static public IMvvmCoroutineExecutor mCoroutineExecutor;
        static public IMvvmLoader mLoader;

        static public void Initialize(
            IWindowManager windowManager,
            IMvvmCoroutineExecutor coroutineExecutor,
            IMvvmLoader loader,
            IMvvmLog mvvmLog = null,
            ISnkMvvmSettings settings = null
            )
        {
            SnkBindingSetup.Initialize();
            
            mSettings = settings ??= new SnkMvvmSettings();
            mMvvmLog = mvvmLog;
            mLoader = loader;
            mWindowManager = windowManager;
            mCoroutineExecutor = coroutineExecutor;
        }
    }

}