using UnityEngine;
using UnityEngine.SceneManagement;

namespace BFFramework.Runtime.Services
{
    public class BFSceneService : BFServiceBase, IBFSceneService
    {
        private IBFAssetBundleService _assetBundleService;
        public BFSceneService(IBFAssetBundleService assetBundleService)
        {
            this._assetBundleService = assetBundleService;
        }
        
        public AsyncOperation LoadSceneAsync(string sceneName, bool additive)
        {
            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            return operation;
        }

        public AsyncOperation UnloadCurrScene()
        {
            var currScene = SceneManager.GetActiveScene();
            return UnloadSceneAsync(currScene.name);
        }

        public AsyncOperation UnloadSceneAsync(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}