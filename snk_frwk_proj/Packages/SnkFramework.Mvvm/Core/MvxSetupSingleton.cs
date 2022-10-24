using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Core.Log;
using UnityEngine;

namespace SnkFramework.Mvvm.Core
{
    public abstract class MvxSetupSingleton
    {
        protected static readonly ISnkMvvmLogger log = SnkIoCProvider.Instance.Resolve<ISnkMvvmLogger>();
        private static readonly object LockObject = new object();
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;

        private ISnkMvvmSetup _setup;
        private ISnkSetupMonitor _currentMonitor;

        protected virtual ISnkMvvmSetup Setup => _setup;

        
        static public MvxSetupSingleton Instance { get; protected set; }
        protected MvxSetupSingleton()
        {
            if (Instance != null)
                throw new System.Exception("You cannot create more than one instance of MvxSingleton");

            Instance = this;
        }

        public virtual TSetup PlatformSetup<TSetup>()
            where TSetup : ISnkMvvmSetup
        {
            try
            {
                return (TSetup)Setup;
            }
            catch (System.Exception ex)
            {
                log.ErrorFormat("Unable to cast setup to {0}.\n{1}", typeof(TSetup), ex);
                throw;
            }
        }

        protected virtual void CreateSetup()
        {
            try
            {
                _setup = SnkMvvmSetup.Instance();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Failed to create setup instance");
            }
        }
        
        protected static TMvxSetupSingleton EnsureSingletonAvailable<TMvxSetupSingleton>()
            where TMvxSetupSingleton : MvxSetupSingleton, new()
        {
            
            Debug.Log("EnsureSingletonAvailable");
            // Double null - check before creating the setup singleton object
            if (Instance != null)
                return Instance as TMvxSetupSingleton;
            lock (LockObject)
            {
                if (Instance != null)
                    return Instance as TMvxSetupSingleton;

                // Go ahead and create the setup singleton, and then
                // create the setup instance. 
                // Note that the Instance property is set within the 
                // singleton constructor
                var instance = new TMvxSetupSingleton();
                instance.CreateSetup();
                return Instance as TMvxSetupSingleton;
            }
        }
        
        public virtual void InitializeAndMonitor(ISnkSetupMonitor setupMonitor)
        {
            Debug.Log("InitializeAndMonitor:" + setupMonitor);
            lock (LockObject)
            {
                _currentMonitor = setupMonitor;

                // if the tcs is not null, it means the initialization is running
                if (IsInitialisedTaskCompletionSource != null)
                {
                    // If the task is already completed at this point, let the monitor know it has finished. 
                    // but don't do it otherwise because it's done elsewhere
                    if (IsInitialisedTaskCompletionSource.Task.IsCompleted)
                    {
                        _currentMonitor?.InitializationComplete();
                    }

                    return;
                }

                StartSetupInitialization();
            }
        }
        private void StartSetupInitialization()
        {
            IsInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
            _setup.InitializePrimary();
            Task.Run(async () =>
            {
                ExceptionDispatchInfo setupException = null;
                try
                {
                    _setup.InitializeSecondary();
                }
                catch(Exception ex)
                {
                    setupException = ExceptionDispatchInfo.Capture(ex);
                }
                
                ISnkSetupMonitor monitor;
                lock (LockObject)
                {
                    if (setupException == null)
                    {
                        IsInitialisedTaskCompletionSource.SetResult(true);
                    }
                    else
                    {
                        IsInitialisedTaskCompletionSource.SetException(setupException.SourceException);
                    }
                    monitor = _currentMonitor;
                }
                await monitor?.InitializationComplete();
            });
        }
    }
}