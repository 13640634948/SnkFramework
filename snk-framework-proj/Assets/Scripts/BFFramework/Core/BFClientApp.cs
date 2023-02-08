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
            InitializeService();
        }

        protected virtual void InitializeService()
        {
            this.RegisterService<IBFAssetBundleService, BFAssetBundleService>();
            this.RegisterService<IBFAssetService, BFAssetService>();
            this.RegisterService<IBFPatchService, BFPatchService>();
        }

        protected void RegisterService<TService, TSvrInstance>()
            where TSvrInstance : class, IBFService
        {
            SnkLogHost.Default?.Info("reg_service:" + typeof(TService) + "启动，实例:" + typeof(TSvrInstance));
            var service = Snk.IoCProvider.IoCConstruct<TSvrInstance>();
            Snk.IoCProvider.RegisterSingleton(typeof(TService), service);
        }
    }
}