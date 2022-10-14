using System;
using SnkFramework.FluentBinding.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.Core
{
    public abstract class SnkMvvmSetup : ISnkMvvmSetup
    {
        public event EventHandler<SnkMvvmSetupStateEventArgs>? StateChanged;
        private SnkMvvmSetupState _state;

        private SnkIoCProvider _iocProvider;

        public SnkMvvmSetupState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(this, new SnkMvvmSetupStateEventArgs(_state));
            }
        }
        protected static Func<ISnkMvvmSetup>? SetupCreator { get; set; }

        public static void RegisterSetupType<TMvxSetup>() 
            where TMvxSetup : SnkMvvmSetup, new()
        => SetupCreator = () => new TMvxSetup();
        
        public static ISnkMvvmSetup Instance()
        {
            var instance = SetupCreator?.Invoke();
            if (instance == null)
                throw new Exception("MvvmSetup 创建失败");
            return instance;
        }

        protected static Action<SnkIoCProvider>? RegisterSetupDependencies { get; set; }

        protected static void ValidateArguments(SnkIoCProvider iocProvider)
        {
            if (iocProvider == null)
                throw new ArgumentNullException(nameof(iocProvider));
        }

        protected virtual void RegisterDefaultSetupDependencies(SnkIoCProvider iocProvider)
        {
            this.InitializeFluentBinding(iocProvider);
        }

        protected virtual void InitializeLoggingServices()
        {
        }

        protected virtual void InitializeFirstChance(SnkIoCProvider iocProvider)
        {
            // always the very first thing to get initialized - after IoC and base platform
            // base class implementation is empty by default
        }

        protected virtual SnkIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = SnkIoCProvider.Instance;
            iocProvider.Register(iocProvider);
            iocProvider.Register<ISnkMvvmSetup>(this);
            return iocProvider;
        }

        protected virtual void InitializeFluentBinding(SnkIoCProvider iocProvider)
        {
            SnkBindingSetup.Initialize();
        }

        protected virtual ISnkMvvmSettings InitializeSettings(SnkIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var settings = CreateSettings(iocProvider);
            return settings;
        }

        protected virtual ISnkMvvmSettings CreateSettings(SnkIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ISnkMvvmSettings>();
        }


        protected virtual void InitializeCache(SnkIoCProvider iocProvider)
        {
        }

        public virtual void InitializePrimary()
        {
            if (State != SnkMvvmSetupState.Uninitialized)
                return;
            State = SnkMvvmSetupState.InitializingPrimary;
            _iocProvider = InitializeIoC();
            InitializeLoggingServices();

            RegisterDefaultSetupDependencies(_iocProvider);
            RegisterSetupDependencies?.Invoke(_iocProvider);

            InitializeFirstChance(_iocProvider);
            InitializeSettings(_iocProvider);

            InitializeCache(_iocProvider);

            State = SnkMvvmSetupState.InitializedPrimary;
        }

        protected virtual void InitializeLastChance(SnkIoCProvider iocProvider)
        {
            // always the very last thing to get initialized
            // base class implementation is empty by default
        }

        protected abstract void InitializeMvvmManager(SnkIoCProvider iocProvider);

        protected abstract void InitializeMvvmCoroutineExecutor(SnkIoCProvider iocProvider);

        protected abstract void InitializeTransitionExecutor(SnkIoCProvider iocProvider);

        protected abstract void InitializeMvvmLoader(SnkIoCProvider iocProvider);

        public void InitializeSecondary()
        {
            if (State != SnkMvvmSetupState.InitializedPrimary)
                return;

            State = SnkMvvmSetupState.InitializingSecondary;

            try
            {
                InitializeMvvmManager(_iocProvider);
                InitializeMvvmCoroutineExecutor(_iocProvider);
                InitializeTransitionExecutor(_iocProvider);
                InitializeMvvmLoader(_iocProvider);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
            InitializeLastChance(_iocProvider);
            State = SnkMvvmSetupState.Initialized;
        }
    }
}