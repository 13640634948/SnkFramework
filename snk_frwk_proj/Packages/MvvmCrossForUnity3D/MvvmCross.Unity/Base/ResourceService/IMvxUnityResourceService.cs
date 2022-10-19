using UnityEngine;

namespace MvvmCross.Unity.Base.ResourceService
{
    public interface IMvxUnityResourceService
    {
        public UnityEngine.Object LoadBuildInResource(string path);
        public TAsset LoadBuildInResource<TAsset>(string path) where TAsset : UnityEngine.Object;
        public ResourceRequest LoadBuildInResourceAsync(string path, System.Action<Object> onCompleted);
        public ResourceRequest LoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted) where TAsset : Object;
    }
}