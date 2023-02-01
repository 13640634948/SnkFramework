using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.IoC;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public partial class SnkSetup
        {
            
            protected virtual ISnkPluginManager CreatePluginManager(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);
                return iocProvider.Resolve<ISnkPluginManager>();
            }
            
            private static bool AssemblyReferencesMvvmCross(Assembly assembly, string mvvmCrossAssemblyName)
            {
                try
                {
                    return assembly.GetReferencedAssemblies().Any(a => a.Name == mvvmCrossAssemblyName);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return false;
                }
            }
            
            
            protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin)
            {
                return null;
            }
            
            public virtual IEnumerable<Assembly> GetPluginAssemblies()
            {
                var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                return assemblies
                    .AsParallel()
                    .Where(assembly => AssemblyReferencesMvvmCross(assembly, mvvmCrossAssemblyName));
            }
            
            public virtual void LoadPlugins(ISnkPluginManager pluginManager)
            {
                if (pluginManager == null)
                    throw new ArgumentNullException(nameof(pluginManager));

                var pluginAttribute = typeof(MvxPluginAttribute);
                var pluginAssemblies = GetPluginAssemblies();

                // Search Assemblies for Plugins
                foreach (var pluginAssembly in pluginAssemblies)
                {
                    var assemblyTypes = pluginAssembly.ExceptionSafeGetTypes();

                    // Search Types for Valid Plugin
                    foreach (var type in assemblyTypes.Where(TypeContainsPluginAttribute))
                    {
                        // Ensure Plugin has been loaded
                        pluginManager.EnsurePluginLoaded(type);
                    }
                }

                bool TypeContainsPluginAttribute(Type type) =>
                    type.GetCustomAttributes(pluginAttribute, false).Length > 0;
            }
            protected virtual ISnkPluginManager InitializePluginFramework(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);

                var pluginManager = CreatePluginManager(iocProvider);
                LoadPlugins(pluginManager);
                return pluginManager;
            }
            protected virtual IMvxApplication CreateMvxApplication(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);
                return iocProvider.Resolve<IMvxApplication>();
            }
            protected virtual IMvxApplication InitializeMvxApplication(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);

                var app = CreateMvxApplication(iocProvider);
                //iocProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
                return app;
            }
            protected virtual void InitializeApp(ISnkPluginManager pluginManager, IMvxApplication app)
            {
                if (app == null)
                    throw new ArgumentNullException(nameof(app));

                app.LoadPlugins(pluginManager);
                this.logger.Info("Setup: Application Initialize - On background thread");
                app.Initialize();
            }
            
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
                    
                    //SetupLog?.Log(LogLevel.Trace, "Setup: Bootstrap actions");
                    //PerformBootstrapActions();
                    //SetupLog?.Log(LogLevel.Trace, "Setup: StringToTypeParser start");
                    //InitializeStringToTypeParser(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: FillableStringToTypeParser start");
                    //InitializeFillableStringToTypeParser(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: PluginManagerFramework start");
                    var pluginManager = InitializePluginFramework(_iocProvider);
                    this.logger?.Info("Setup: Create App");
                    var app = InitializeMvxApplication(_iocProvider);
                    this.logger.Info("Setup: NavigationService");
                    InitializeMvvmService(_iocProvider);
                    this.logger?.Info("Setup: App start");
                    InitializeApp(pluginManager, app);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
                    //InitializeViewModelTypeFinder(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
                    //InitializeViewsContainer(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
                    //var lookup = InitializeLookupDictionary(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                    //InitializeViewLookup(lookup, _iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: CommandCollectionBuilder start");
                    //InitializeCommandCollectionBuilder(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: NavigationSerializer start");
                    //InitializeNavigationSerializer(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                    //InitializeInpcInterception(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                    //InitializeViewModelCache(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: LastChance start");
                  
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