
using System;
using System.Collections.Generic;

namespace SnkFramework.Plugins
{
    public interface ISnkPluginManager
    {
        Func<Type, IMvxPluginConfiguration?> ConfigurationSource { get; }

        IEnumerable<Type> LoadedPlugins { get; }

        bool IsPluginLoaded(Type type);

        bool IsPluginLoaded<TPlugin>() where TPlugin : ISnkPlugin;

        void EnsurePluginLoaded(Type type, bool forceLoad = false);

        void EnsurePluginLoaded<TPlugin>(bool forceLoad = false)
            where TPlugin : ISnkPlugin;

        bool TryEnsurePluginLoaded(Type type, bool forceLoad = false);

        bool TryEnsurePluginLoaded<TPlugin>(bool forceLoad = false)
            where TPlugin : ISnkPlugin;
    }
}