using SnkFramework.IoC;
using SnkFramework.Runtime;

namespace BFFramework.Runtime.Managers
{
    public class BFManagerHub
    {
        public static void Initialize(ISnkIoCProvider iocProvider)
        {
            registerManager<IBFModuleManager, BFModuleManager>(iocProvider);
            registerManager<IBFCameraManager, BFCameraManager>(iocProvider);
        }
        
        private static void registerManager<TManager, TMgrInstance>(ISnkIoCProvider iocProvider)
            where TMgrInstance : class, IBFManager
        {
            SnkLogHost.Default?.Info("Setup: Register " + typeof(TMgrInstance));
            var service = iocProvider.IoCConstruct<TMgrInstance>();
            iocProvider.RegisterSingleton(typeof(TManager), service);
        }
    }
}