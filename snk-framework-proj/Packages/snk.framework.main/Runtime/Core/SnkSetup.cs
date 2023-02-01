using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.NuGet.Exceptions;
using SnkFramework.NuGet.Features.Logging;
using UnityEngine;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public abstract partial class SnkSetup : ISnkSetup
        {
            private static readonly object _lock = new();

            private TaskCompletionSource<bool> _isInitialisedTaskCompletionSource;
            //private SynchronizationContext _unitySynchronizationContext;

            private ISnkSetupMonitor _monitor;

            //public static Func<ISnkSetupMonitor> MonitorGetter = null;
            public event EventHandler<SnkSetupStateEventArgs> StateChanged;

            private eSnkSetupState _state;

            private ISnkIoCProvider _iocProvider;
            protected ISnkLogger logger;
            public eSnkSetupState State
            {
                get => _state;
                private set
                {
                    if(_state == value)
                        return;
                    _state = value;
                    FireStateChange(value);
                }
            }
            
            
            protected static Func<ISnkSetup>? SetupCreator { get; set; }

            public static void RegisterSetupType<TMvxSetup>() where TMvxSetup : SnkSetup, new()
            {
                // We are using double-checked locking here to avoid overhead of locking if the
                // SetupCreator is already created
                if (SetupCreator is null)
                {
                    lock (_lock)
                    {
                        if (SetupCreator is null)
                        {


                            // Avoid creating the instance of Setup right now, instead
                            // take a reference to the type in a way that we can avoid
                            // using reflection to create the instance.
                            SetupCreator = () => new TMvxSetup();

                            return;
                        }
                    }
                }
                //MvxLogHost.Default?.LogInformation("Setup: RegisterSetupType already called");
            }

            public static ISnkSetup Instance()
            {
                var instance = SetupCreator?.Invoke();
                if (instance == null)
                {
                    //instance = MvxSetupExtensions.CreateSetup<MvxSetup>();
                    throw new SnkException("Could not find a Setup class for application");
                }
                return instance;
            }
            
            private void FireStateChange(eSnkSetupState state)
            {
                StateChanged?.Invoke(this, new SnkSetupStateEventArgs(state));
            }
            
            protected virtual void InitializeFirstChance(ISnkIoCProvider iocProvider)
            {
                // always the very first thing to get initialized - after IoC and base platform
                // base class implementation is empty by default
            }
            
            protected virtual void InitializeLastChance(ISnkIoCProvider iocProvider)
            {
                // always the very last thing to get initialized
                // base class implementation is empty by default
            }
            
            public async void Startup(ISnkSetupMonitor setupMonitor)
            {
                InitializeAndMonitor(setupMonitor);

                await Task.WhenAny(_isInitialisedTaskCompletionSource.Task);

                lock (_lock)
                {
                    if (!_isInitialisedTaskCompletionSource.Task.IsCompleted) return;
                    if (!_isInitialisedTaskCompletionSource.Task.IsFaulted) return;
                    Debug.LogException(_isInitialisedTaskCompletionSource.Task.Exception);
                    Debug.LogError("初始化游戏失败");
                }
            }

            protected virtual void InitializeAndMonitor(ISnkSetupMonitor setupMonitor)
            {
                lock (_lock)
                {
                    if (setupMonitor == null)
                    {
                        if (_isInitialisedTaskCompletionSource == null)
                        {
                            StartSetupInitialization();
                        }
                        else
                        {
                            if (_isInitialisedTaskCompletionSource.Task.IsCompleted)
                            {
                                if (_isInitialisedTaskCompletionSource.Task.IsFaulted)
                                    throw _isInitialisedTaskCompletionSource.Task.Exception;
                                return;
                            }
                            Debug.LogError("EnsureInitialized has already been called so now waiting for completion");
                        }
                    }
                    else
                    {
                        _monitor = setupMonitor;
                        // if the tcs is not null, it means the initialization is running
                        if (_isInitialisedTaskCompletionSource != null)
                        {
                            // If the task is already completed at this point, let the monitor know it has finished. 
                            // but don't do it otherwise because it's done elsewhere
                            if (_isInitialisedTaskCompletionSource.Task.IsCompleted)
                            {
                                _monitor?.InitializationComplete();
                            }
                            return;
                        }
                        StartSetupInitialization();
                    }
                }
            }

            private void StartSetupInitialization()
            {
                _isInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
                InitializePrimary();
                Task.Run(async () =>
                {
                    ExceptionDispatchInfo setupException = null;
                    try
                    {
                        InitializeSecondary();
                    }
                    catch (Exception ex)
                    {
                        setupException = ExceptionDispatchInfo.Capture(ex);
                    }

                    ISnkSetupMonitor monitor;
                    lock (_lock)
                    {
                        if (setupException == null)
                        {
                            _isInitialisedTaskCompletionSource.SetResult(true);
                        }
                        else
                        {
                            _isInitialisedTaskCompletionSource.SetException(setupException.SourceException);
                        }

                        monitor = _monitor;
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
}