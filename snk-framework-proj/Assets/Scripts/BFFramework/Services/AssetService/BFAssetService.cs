namespace BFFramework.Runtime.Services
{
    public class BFAssetService : BFServiceBase, IBFAssetService
    {
        private IBFAssetBundleService _assetBundleService;
        public BFAssetService(IBFAssetBundleService assetBundleService)
        {
            this._assetBundleService = assetBundleService;
        }
    }
}