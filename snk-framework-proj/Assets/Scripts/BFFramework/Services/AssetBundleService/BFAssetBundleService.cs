namespace BFFramework.Runtime.Services
{
    public class BFAssetBundleService : BFServiceBase, IBFAssetBundleService
    {
        private IBFPatchService _patchService;
        public BFAssetBundleService(IBFPatchService patchService)
        {
            this._patchService = patchService;
        }
    }
}