using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.View;
using System.Collections;

namespace SnkFramework.Mvvm.Base
{
    public interface IMvvmCoroutineExecutor
    {
        public void RunOnCoroutineNoReturn(IEnumerator routine);
    }
    
    public class SnkMvvmSetup
    {
        static public ISnkMvvmSettings mSettings;
        static public IWindowManager mWindowManager;
        static public IMvvmLog mMvvmLog;
        static public IMvvmCoroutineExecutor mCoroutineExecutor;

        static public void Initialize(
            IWindowManager windowManager,
            IMvvmCoroutineExecutor coroutineExecutor,
            ISnkMvvmSettings settings = null,
            IMvvmLog mvvmLog = null
            )
        {
            SnkBindingSetup.Initialize();
            
            mSettings = settings ??= new SnkMvvmSettings();
            mMvvmLog = mvvmLog ??= new SnkMvvmLog();
            mWindowManager = windowManager;
            mCoroutineExecutor = coroutineExecutor;
        }
    }

}