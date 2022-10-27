using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SnkFramework.Runtime.Core.Setup
{
    public abstract partial class SnkSetupLifetimeScope : LifetimeScope, ISnkSetup
    {
        private readonly object _lock = new();

        private TaskCompletionSource<bool> _isInitialisedTaskCompletionSource;
        private SynchronizationContext _unitySynchronizationContext;

        private ISnkSetupMonitor _monitor;

        public static Func<ISnkSetupMonitor> MonitorGetter = null;

        protected override async void Configure(IContainerBuilder builder)
        {
            _unitySynchronizationContext = SynchronizationContext.Current;

            this._monitor = MonitorGetter?.Invoke();

            if (_monitor == null)
                EnsureInitialized(builder);
            else
                InitializeAndMonitor(builder, _monitor);

            await Task.WhenAny(_isInitialisedTaskCompletionSource.Task);

            lock (_lock)
            {
                if (_isInitialisedTaskCompletionSource.Task.IsCompleted)
                {
                    if (_isInitialisedTaskCompletionSource.Task.IsFaulted)
                    {
                        Debug.LogException(_isInitialisedTaskCompletionSource.Task.Exception);
                        Debug.LogError("初始化游戏失败");
                    }
                }
            }
        }

        public virtual void EnsureInitialized(IContainerBuilder builder)
        {
            lock (_lock)
            {
                if (_isInitialisedTaskCompletionSource == null)
                {
                    StartSetupInitialization(builder);
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
        }

        public virtual void InitializeAndMonitor(IContainerBuilder builder, ISnkSetupMonitor setupMonitor)
        {
            lock (_lock)
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

                StartSetupInitialization(builder);
            }
        }

        private void StartSetupInitialization(IContainerBuilder builder)
        {
            _isInitialisedTaskCompletionSource = new TaskCompletionSource<bool>();
            InitializePrimary(builder);
            Task.Run(() =>
            {
                ExceptionDispatchInfo setupException = null;
                try
                {
                    InitializeSecondary(builder);
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
                    async void SendOrPostCallback(object _)
                    {
                        if (monitor != null)
                        {
                            await monitor.InitializationComplete();
                        }
                    }

                    _unitySynchronizationContext.Post(SendOrPostCallback, null);
                }
            });
        }
    }
}