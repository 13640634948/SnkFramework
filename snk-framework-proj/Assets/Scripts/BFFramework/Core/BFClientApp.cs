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
            this.RegisterService<IBFPatchService, BFPatchService>();
            this.RegisterConstructService<IBFAssetBundleService, IBFPatchService>(patchSvr => new BFAssetBundleService(patchSvr));
            this.RegisterConstructService<IBFAssetService, IBFAssetBundleService>(assetBundleSvr => new BFAssetService(assetBundleSvr));
            this.RegisterConstructService<IBFSceneService, IBFAssetBundleService>(assetBundleSvr => new BFSceneService(assetBundleSvr));
        }
    }
}