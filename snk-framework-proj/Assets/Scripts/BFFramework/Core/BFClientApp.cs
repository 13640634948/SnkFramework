using SnkFramework.Runtime;
using SnkFramework.Runtime.Core;

using BFFramework.Runtime.Services;

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
            this.RegisterService<IBFAssetBundleService, BFAssetBundleService>();
            this.RegisterService<IBFAssetService, BFAssetService>();
            this.RegisterService<IBFSceneService, BFSceneService>();
            this.RegisterService<IBFPatchService, BFPatchService>();
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