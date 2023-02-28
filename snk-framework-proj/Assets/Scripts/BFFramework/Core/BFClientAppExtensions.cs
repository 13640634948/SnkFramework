using System;
using BFFramework.Runtime.Services;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Logging;
using SnkFramework.Runtime;

namespace BFFramework.Runtime.Core
{
    public static class BFClientAppExtensions
    {
        public static void RegisterService<TService, TSvrInstance>(this BFClientApp target)
            where TSvrInstance : class, IBFService
        {
            if(SnkLogHost.Default.IsInfoEnabled)
                SnkLogHost.Default?.Info("Setup: Register {typeof(TSvrInstance)}");
            var service = Snk.IoCProvider.IoCConstruct<TSvrInstance>();
            Snk.IoCProvider.RegisterSingleton(typeof(TService), service);
        }
        
        public static void RegisterConstructService<TInterface, TParameter1>(this BFClientApp target, Func<TParameter1, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
        {
            Snk.IoCProvider.TryResolve(typeof(TParameter1), out var parameter1);
            var instance = constructor((TParameter1)parameter1);
            if(SnkLogHost.Default.IsInfoEnabled)
            SnkLogHost.Default?.Info($"Setup: Register {instance.GetType()}");
            Snk.IoCProvider.RegisterSingleton(instance);
        }
    }
}