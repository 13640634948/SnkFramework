using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.NuGet.Exceptions;
using SnkFramework.NuGet.Features.Logging;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public abstract partial class SnkSetup : ISnkSetup
        {
            private static readonly object _lock = new();

            private TaskCompletionSource<bool> _isInitialisedTaskCompletionSource;

            private ISnkSetupMonitor _monitor;
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

            protected static List<Assembly> AppDomainAssemblyList = new List<Assembly>();

            public static void RegisterSetupType<TMvxSetup>(params Assembly[] assemblies) where TMvxSetup : SnkSetup, new()
            {
                if (SetupCreator is null)
                {
                    lock (_lock)
                    {
                        if (SetupCreator is null)
                        {
                            AppDomainAssemblyList.AddRange(assemblies);
                            if (AppDomainAssemblyList.Count == 0)
                            {
                                // fall back to all assemblies. Assembly.GetEntryAssembly() always returns
                                // null on Xamarin platforms do not use it!
                                AppDomainAssemblyList.AddRange(AppDomain.CurrentDomain.GetAssemblies());
                            }
                            
                            SetupCreator = () => new TMvxSetup();
                        }
                    }
                }
                //MvxLogHost.Default?.LogInformation("Setup: RegisterSetupType already called");
            }

            protected List<Assembly> GetAssembly(string prefix)
                => AppDomainAssemblyList.FindAll(a => a.FullName.StartsWith(prefix));

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
        }
    }
}