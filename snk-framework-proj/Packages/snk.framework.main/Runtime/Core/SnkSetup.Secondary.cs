using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public partial class SnkSetup
        {
            protected virtual ISnkPluginManager CreatePluginManager(ISnkIoCProvider iocProvider)
                => iocProvider.Resolve<ISnkPluginManager>();
            
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

            protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin) => null;
            
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
                var pluginManager = CreatePluginManager(iocProvider);
                LoadPlugins(pluginManager);
                return pluginManager;
            }
            
            protected virtual ISnkApplication CreateSnkApplication(ISnkIoCProvider iocProvider)
                => iocProvider.Resolve<ISnkApplication>();
            
            protected virtual ISnkApplication InitializeSnkApplication(ISnkIoCProvider iocProvider)
                => CreateSnkApplication(iocProvider);
            
            protected virtual void InitializeApp(ISnkPluginManager pluginManager, ISnkApplication app)
            {
                if (app == null)
                    throw new ArgumentNullException(nameof(app));

                app.LoadPlugins(pluginManager);
                this.logger.Info("Setup: Application Initialize - On background thread");
                app.Initialize();
            }

            protected virtual void InitializeAppStart()
            {
                // 这里初始化AppStart
                // AppStart可以作为平台鉴权的路由
                foreach (var type in GetUserInterfaceAssembly().GetTypes())
                {
                    if (type.IsDefined(typeof(SnkAppStartAttribute), false))
                    {
                        var instance = Snk.IoCProvider.IoCConstruct(type);
                        Snk.IoCProvider.RegisterSingleton(instance as ISnkAppStart);
                    }
                }
            }

            
            protected virtual string UserInterfaceAssemblyName { get; } = "";
            
            private Assembly _userInterfaceAssembly;
            protected virtual Assembly GetUserInterfaceAssembly()
            {
                if (UserInterfaceAssemblyName == null)
                    return null;
                if (_userInterfaceAssembly == null)
                    _userInterfaceAssembly = GetAssembly(UserInterfaceAssemblyName)[0];
                return _userInterfaceAssembly;
            }

            protected abstract ISnkNameMapping CreateViewToViewModelNaming();
            
            protected virtual ISnkViewModelByNameLookup CreateViewModelByNameLookup(ISnkIoCProvider iocProvider)
                => iocProvider.Resolve<ISnkViewModelByNameLookup>();
            protected virtual ISnkViewModelByNameRegistry CreateViewModelByNameRegistry(ISnkIoCProvider iocProvider)
                => iocProvider.Resolve<ISnkViewModelByNameRegistry>();

            protected virtual void InitializeViewModelTypeFinder(ISnkIoCProvider iocProvider)
            {
                CreateViewModelByNameLookup(iocProvider);
                var viewModelByNameRegistry = CreateViewModelByNameRegistry(iocProvider);
                viewModelByNameRegistry.AddAll(GetUserInterfaceAssembly());

                var nameMappingStrategy = CreateViewToViewModelNaming();
                iocProvider.RegisterSingleton(nameMappingStrategy);
                //return nameMappingStrategy;
            }
            
            protected virtual IDictionary<Type, Type> InitializeLookupDictionary(ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);
                var viewAssemblies = GetUserInterfaceAssembly();//GetViewAssemblies();
                var builder = iocProvider.Resolve<ISnkTypeToTypeLookupBuilder>();
                return builder.Build(new []{viewAssemblies});
            }
            protected virtual void InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup, ISnkIoCProvider iocProvider)
            {
                //ValidateArguments(iocProvider);

                var container = iocProvider.Resolve<IMvxViewsContainer>();
                container.AddAll(viewModelViewLookup);
                //return container;
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
                    var app = InitializeSnkApplication(_iocProvider);
                    //this.logger.Info("Setup: NavigationService");
                    //InitializeMvvmService(_iocProvider);
                    this.logger?.Info("Setup: App start");
                    InitializeApp(pluginManager, app);
                    InitializeAppStart();
                    //SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
                    InitializeViewModelTypeFinder(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
                    //InitializeViewsContainer(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
                    var lookup = InitializeLookupDictionary(_iocProvider);
                    //SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                    InitializeViewLookup(lookup, _iocProvider);
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