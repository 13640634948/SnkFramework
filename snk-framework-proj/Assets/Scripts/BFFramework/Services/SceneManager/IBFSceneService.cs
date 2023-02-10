using UnityEngine;

namespace BFFramework.Runtime.Services
{
    public interface IBFSceneService
    {
        AsyncOperation LoadSceneAsync(string sceneName, bool additive);
        AsyncOperation UnloadCurrScene();
        AsyncOperation UnloadSceneAsync(string sceneName);
    }
}