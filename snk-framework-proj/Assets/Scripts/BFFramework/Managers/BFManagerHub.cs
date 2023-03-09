using SnkFramework.IoC;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Logging;

namespace BFFramework.Runtime.Managers
{
    public class BFManagerHub
    {
        public static void Initialize(ISnkIoCProvider iocProvider)
        {
            registerManager<IBFModuleManager, BFModuleManager>(iocProvider);
            registerManager<IBFCameraManager, BFCameraManager>(iocProvider);
            registerManager<IBFAsyncManager, BFAsyncManager>(iocProvider);
            registerManager<IBFDownloadManager, BFDownloadManager>(iocProvider);
        }
        
        private static void registerManager<TManager, TMgrInstance>(ISnkIoCProvider iocProvider)
            where TMgrInstance : class, IBFManager
        {
            if(SnkLogHost.Default.IsInfoEnabled)
                SnkLogHost.Default?.Info("Setup: Register " + typeof(TMgrInstance));
            var service = iocProvider.IoCConstruct<TMgrInstance>();
            iocProvider.RegisterSingleton(typeof(TManager), service);
        }
    }
}