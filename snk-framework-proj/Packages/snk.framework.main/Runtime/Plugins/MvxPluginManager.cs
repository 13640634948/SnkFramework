using System;
using System.Collections.Generic;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.Plugins
{
    public class MvxPluginManager : ISnkPluginManager
    {
       private readonly object _lockObject = new object();
        private readonly HashSet<Type> _loadedPlugins = new HashSet<Type>();

        public Func<Type, IMvxPluginConfiguration?> ConfigurationSource { get; }

        public IEnumerable<Type> LoadedPlugins => _loadedPlugins;

        public MvxPluginManager(Func<Type, IMvxPluginConfiguration?> configurationSource)
        {
            ConfigurationSource = configurationSource;
        }

        public void EnsurePluginLoaded<TPlugin>(bool forceLoad = false) where TPlugin : ISnkPlugin
        {
            EnsurePluginLoaded(typeof(TPlugin), forceLoad);
        }

        public virtual void EnsurePluginLoaded(Type type, bool forceLoad = false)
        {
            if (!forceLoad && IsPluginLoaded(type))
                return;

            var plugin = Activator.CreateInstance(type) as ISnkPlugin;
            if (plugin == null)
                throw new SnkException($"Type {type} is not an IMvxPlugin");

            if (plugin is IMvxConfigurablePlugin configurablePlugin)
            {
                var configuration = ConfigurationFor(type);
                if (configuration != null)
                    configurablePlugin.Configure(configuration);
            }

            plugin.Load();

            lock (_lockObject)
            {
                _loadedPlugins.Add(type);
            }
        }

        protected IMvxPluginConfiguration? ConfigurationFor(Type toLoad) =>
            ConfigurationSource.Invoke(toLoad);

        public bool IsPluginLoaded<T>() where T : ISnkPlugin
            => IsPluginLoaded(typeof(T));

        public bool IsPluginLoaded(Type type)
        {
            lock (_lockObject)
            {
                return _loadedPlugins.Contains(type);
            }
        }

        public bool TryEnsurePluginLoaded<TPlugin>(bool forceLoad = false) where TPlugin : ISnkPlugin
            => TryEnsurePluginLoaded(typeof(TPlugin), forceLoad);

        public bool TryEnsurePluginLoaded(Type type, bool forceLoad = false)
        {
            try
            {
                EnsurePluginLoaded(type, forceLoad);
                return true;
            }
            catch (Exception ex)
            {
                //MvxLogHost.Default?.Log(LogLevel.Warning, ex, "Failed to load plugin {fullPluginName}", type.FullName);
                return false;
            }
        }
    }
}