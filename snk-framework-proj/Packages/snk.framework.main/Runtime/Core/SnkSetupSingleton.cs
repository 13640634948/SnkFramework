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
        private static readonly object s_locker = new object();

        //private static TaskCompletionSource<bool> IsInitialisedTaskCompletionSource;
        private ISnkSetup _setup;
        private ISnkSetupMonitor _currMonitor;

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
                SnkLogHost.Default?.Exception(ex, "Unable to cast setup to {0}", typeof(TSnkSetup));
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
            lock (s_locker)
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


        public virtual void CancelMonitor(ISnkSetupMonitor setupMonitor)
        {
            lock (s_locker)
            {
                if (setupMonitor != _currMonitor)
                {
                    throw new SnkException(
                        "The specified ISnkSetupMonitor is not the one registered in SnkSetupSingleton");
                }

                _currMonitor = null;
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
                lock (s_locker)
                {
                    _currMonitor = null;
                }
            }

            base.Dispose(isDisposing);
        }

        public async Task EnsureInitialized(ISnkSetupMonitor monitor)
        {
            _currMonitor = monitor;
            await StartSetupInitialization();
        }

        private async Task StartSetupInitialization()
        {
            _setup.InitializePrimary();
            await Task.Run(async () =>
            {
                ExceptionDispatchInfo setupException = null;
                try
                {
                    await _setup.InitializeSecondary();
                }
                catch (Exception ex)
                {
                    setupException = ExceptionDispatchInfo.Capture(ex);
                }

                if (setupException != null)
                    throw setupException.SourceException;
                
                ISnkSetupMonitor monitor;
                lock (s_locker)
                {
                    monitor = _currMonitor;
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