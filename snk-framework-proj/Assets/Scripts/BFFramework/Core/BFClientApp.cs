using SnkFramework.Runtime;
using SnkFramework.Runtime.Core;
using BFFramework.Runtime.Services;
using SnkFramework.IoC;

namespace BFFramework.Runtime.Core
{
    public abstract class BFClientApp : SnkApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterServices();
        }

        protected virtual void RegisterServices()
        {
            this.RegisterService<IBFPatchService, BFPatchService>();
            Snk.IoCProvider.LazyConstructAndRegisterSingleton<IBFAssetBundleService, IBFPatchService>(
                patchSvr => new BFAssetBundleService(patchSvr));
            Snk.IoCProvider.LazyConstructAndRegisterSingleton<IBFAssetService, IBFAssetBundleService>(
                assetBundleSvr => new BFAssetService(assetBundleSvr));
            Snk.IoCProvider.LazyConstructAndRegisterSingleton<IBFSceneService, IBFAssetBundleService>(
                assetBundleSvr => new BFSceneService(assetBundleSvr));
        }

        protected void RegisterService<TService, TSvrInstance>()
            where TSvrInstance : class, IBFService
        {
            SnkLogHost.Default?.Info("Setup: Register " + typeof(TSvrInstance));
            var service = Snk.IoCProvider.IoCConstruct<TSvrInstance>();
            Snk.IoCProvider.RegisterSingleton(typeof(TService), service);
        }
    }
}