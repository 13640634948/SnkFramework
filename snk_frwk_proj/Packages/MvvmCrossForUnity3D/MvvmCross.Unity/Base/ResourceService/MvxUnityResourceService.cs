using System.Threading.Tasks;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MvvmCross.Unity.Base.ResourceService
{
    public class MvxUnityResourceService : IMvxUnityResourceService
    {
        public Object LoadBuildInResource(string path)
            => Resources.Load(path);

        public TAsset LoadBuildInResource<TAsset>(string path) where TAsset : Object
            => Resources.Load<TAsset>(path);

        public async Task<Object> LoadBuildInResourceAsync(string path)
            => await Resources.LoadAsync<Object>(path);

        public async Task<TAsset> LoadBuildInResourceAsync<TAsset>(string path) where TAsset : Object
            => await Resources.LoadAsync<TAsset>(path) as TAsset;

        public async void LoadBuildInResourceAsync(string path, System.Action<Object> onCompleted)
            => onCompleted?.Invoke(await LoadBuildInResourceAsync(path));

        public async void LoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted)
            where TAsset : Object
            => onCompleted?.Invoke(await LoadBuildInResourceAsync(path) as TAsset);

        public IEnumerator CoLoadBuildInResourceAsync(string path, System.Action<Object> onCompleted)
        {
            if(onCompleted == null)
                yield break;
            ResourceRequest request = Resources.LoadAsync(path);
            yield return request;
            onCompleted?.Invoke(request.asset);
        }

        public IEnumerator CoLoadBuildInResourceAsync<TAsset>(string path, System.Action<TAsset> onCompleted)
            where TAsset : Object
        {
            if(onCompleted == null)
                yield break;
            yield return CoLoadBuildInResourceAsync(path, asset => onCompleted?.Invoke(asset as TAsset));
        }
    }
}