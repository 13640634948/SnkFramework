using System;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public partial class SnkSetup
        {
            public void InitializeSecondary()
            {
                if (State != eSnkSetupState.InitializedPrimary)
                {
                    logger?.Error($"InitializeSecondary() called when State is not InitializedPrimary. State: {State}");
                    return;
                }

                if (_iocProvider == null)
                {
                    logger?.Error("InitializeSecondary() IoC Provider is null");
                    throw new InvalidOperationException("Cannot continue setup with null IoCProvider");
                }

                try
                {
                    State = eSnkSetupState.InitializingSecondary;
                    /*
                    SetupLog?.Log(LogLevel.Trace, "Setup: Bootstrap actions");
                    PerformBootstrapActions();
                    SetupLog?.Log(LogLevel.Trace, "Setup: StringToTypeParser start");
                    InitializeStringToTypeParser(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: FillableStringToTypeParser start");
                    InitializeFillableStringToTypeParser(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: PluginManagerFramework start");
                    var pluginManager = InitializePluginFramework(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: Create App");
                    var app = InitializeMvxApplication(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: NavigationService");
                    InitializeNavigationService(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: App start");
                    InitializeApp(pluginManager, app);
                    SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
                    InitializeViewModelTypeFinder(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
                    InitializeViewsContainer(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
                    var lookup = InitializeLookupDictionary(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                    InitializeViewLookup(lookup, _iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: CommandCollectionBuilder start");
                    InitializeCommandCollectionBuilder(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: NavigationSerializer start");
                    InitializeNavigationSerializer(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                    InitializeInpcInterception(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                    InitializeViewModelCache(_iocProvider);
                    SetupLog?.Log(LogLevel.Trace, "Setup: LastChance start");
                    */
                    InitializeLastChance(_iocProvider);
                    logger?.Info("Setup: Secondary end");
                    State = eSnkSetupState.Initialized;
                }
                catch (Exception e)
                {
                    logger?.Exception(e, "InitializeSecondary() failed initializing secondary dependencies");
                    throw;
                }
            }
        }
    }
}