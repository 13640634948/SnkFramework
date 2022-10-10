using System;
using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.View;
using System.Collections;
using SampleDevelop.Test;

namespace SnkFramework.Mvvm.Base
{
    public interface IMvvmLoader
    {
        public ISnkViewOwner LoadViewOwner(string ownerPath);
        public IEnumerator LoadViewOwnerAsync(string ownerPath, Action<ISnkViewOwner> callback);
    }

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
        static public IMvvmLoader mLoader;

        static public void Initialize(
            IWindowManager windowManager,
            IMvvmCoroutineExecutor coroutineExecutor,
            IMvvmLoader loader,
            ISnkMvvmSettings settings = null,
            IMvvmLog mvvmLog = null
            )
        {
            SnkBindingSetup.Initialize();
            
            mSettings = settings ??= new SnkMvvmSettings();
            mMvvmLog = mvvmLog ??= new SnkMvvmLog();
            mLoader = loader;
            mWindowManager = windowManager;
            mCoroutineExecutor = coroutineExecutor;
        }
    }

}