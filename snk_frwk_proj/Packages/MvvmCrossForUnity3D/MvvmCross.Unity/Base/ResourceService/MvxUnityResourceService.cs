using UnityEngine;

namespace MvvmCross.Unity.Base.ResourceService
{
    public class MvxUnityResourceService : IMvxUnityResourceService
    {
        public virtual Object LoadBuildInResource(string path) => Resources.Load(path);

        public virtual TAsset LoadBuildInResource<TAsset>(string path) where TAsset : Object
            => LoadBuildInResource(path) as TAsset;

        public virtual ResourceRequest LoadBuildInResourceAsync(string path, System.Action<Object> onCompleted)
        {
            ResourceRequest request = Resources.LoadAsync(path);
            request.completed += operation => onCompleted?.Invoke(request.asset);
            return request;
        }

        public virtual ResourceRequest LoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted)
            where TAsset : Object
            => LoadBuildInResourceAsync(path, asset => onCompleted(asset as TAsset));
    }
}