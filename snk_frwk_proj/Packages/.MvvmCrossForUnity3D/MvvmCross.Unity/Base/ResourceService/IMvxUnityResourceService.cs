using System.Collections;
using System.Threading.Tasks;

namespace MvvmCross.Unity.Base.ResourceService
{
    public interface IMvxUnityResourceService
    {
        public UnityEngine.Object LoadBuildInResource(string path);
        public TAsset LoadBuildInResource<TAsset>(string path) where TAsset : UnityEngine.Object;
        public Task<UnityEngine.Object> LoadBuildInResourceAsync(string path);
        public Task<TAsset> LoadBuildInResourceAsync<TAsset>(string path) where TAsset : UnityEngine.Object;
        public void LoadBuildInResourceAsync(string path, System.Action<UnityEngine.Object> onCompleted);
        public void LoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted) where TAsset : UnityEngine.Object;
        public IEnumerator CoLoadBuildInResourceAsync(string path, System.Action<UnityEngine.Object> onCompleted);
        public IEnumerator CoLoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted) where TAsset : UnityEngine.Object;
    }
}