using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SnkFramework.Runtime.Core.Setup;
using UnityEngine;

namespace DefaultNamespace
{
    public class SplashScreen : SnkSplashScreen<SetupLifetimeScope>
    {
        [SerializeField] private bool completed = false;

        public override async Task InitializationComplete()
        {
            await UniTask.WaitUntil(() => completed);
            Debug.Log("MvxSetupMonitor-InitializationComplete");
        }
    }
}