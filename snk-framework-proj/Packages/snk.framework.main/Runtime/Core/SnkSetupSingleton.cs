using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.Runtime.Core
{
    public abstract class SnkSetupSingleton
        : SnkSingleton<SnkSetupSingleton>
    {
              private static readonly object LockObject = new object();
        private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private ISnkSetup _setup;
        private ISnkSetupMonitor _currentMonitor;

        protected virtual ISnkSetup Setup => _setup;

        /// <summary>
        /// Returns a platform specific instance of Setup
        /// A useful overload to allow for platform specific
        /// setup logic to be invoked.
        /// </summary>
        /// <typeparam name="TSnkSetup">The platform specific setup type</typeparam>
        /// <returns>A platform specific instance of Setup</returns>
        public virtual TSnkSetup PlatformSetup<TSnkSetup>()
            where TSnkSetup : ISnkSetup
        {
            try
            {
                return (TSnkSetup)Setup;
            }
            catch (Exception ex)
            {
                SnkLogHost.Default?.Exception( ex, "Unable to cast setup to {0}", typeof(TSnkSetup));
                throw;
            }
        }

        /// <summary>
        /// Returns a singleton object that is used to manage the creation and
        /// execution of setup
        /// </summary>
        /// <typeparam name="TSnkSetupSingleton">The platform specific setup singleton type</typeparam>
        /// <returns>A platform specific setup singleton</returns>
        protected static TSnkSetupSingleton EnsureSingletonAvailable<TSnkSetupSingleton>()
           where TSnkSetupSingleton : SnkSetupSingleton, new()
        {
            // Double null - check before creating the setup singleton object
            if (Instance != null)
                return Instance as TSnkSetupSingleton;
            lock (LockObject)
            {
                if (Instance != null)
                    return Instance as TSnkSetupSingleton;

                // Go ahead and create the setup singleton, and then
                // create the setup instance. 
                // Note that the Instance property is set within the 
                // singleton constructor
                var instance = new TSnkSetupSingleton();
                instance.CreateSetup();
                return Instance as TSnkSetupSingleton;
            }
        }

        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (IsInitialisedTaskCompletionSource == null)
                {
                    StartSetupInitialization();
                }
                else
                {
                    if (IsInitialisedTaskCompletionSource.Task.IsCompleted)
                    {
                        if (IsInitialisedTaskCompletionSource.Task.IsFaulted)
                            throw IsInitialisedTaskCompletionSource.Task.Exception;
                        return;
                    }

                    SnkLogHost.Default?.Warning("EnsureInitialized has already been called so now waiting for completion");
                }
            }
            IsInitialisedTaskCompletionSource.Task.GetAwaiter().GetResult();
        }

        public virtual void InitializeAndMonitor(ISnkSetupMonitor setupMonitor)
        {
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

        public virtual void CancelMonitor(ISnkSetupMonitor setupMonitor)
        {
            lock (LockObject)
            {
                if (setupMonitor != _currentMonitor)
                {
                    throw new SnkException("The specified ISnkSetupMonitor is not the one registered in SnkSetupSingleton");
                }
                _currentMonitor = null;
            }
        }

        protected virtual void CreateSetup()
        {
            try
            {
                _setup = SnkSetup.Instance();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create setup instance");
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                lock (LockObject)
                {
                    _currentMonitor = null;
                }
            }
            base.Dispose(isDisposing);
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
                catch (Exception ex)
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

                if (monitor != null)
                {
                    var dispatcher = Snk.IoCProvider.GetSingleton<ISnkMainThreadAsyncDispatcher>();
                    await dispatcher.ExecuteOnMainThreadAsync(async () =>
                    {
                        if (monitor != null)
                        {
                            await monitor.InitializationComplete();
                        }
                    });
                }
            });
        }
    }
}