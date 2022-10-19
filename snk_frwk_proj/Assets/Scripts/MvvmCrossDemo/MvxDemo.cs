using System.Threading;
using MvvmCross.Core;
using MvvmCross.Unity.Core;
using MvvmCross.ViewModels;
using UnityEngine;

namespace MvvmCross.Demo
{

    public class MvxDemo : MonoBehaviour
    {
        void Start()
        {
            MvxSetup.RegisterSetupType<DemoSetup>();
            var singleton = MvxUnitySetupSingleton.EnsureSingletonAvailable(SynchronizationContext.Current);
            singleton.EnsureInitialized();
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                startup.StartAsync();
            }
            else
            {
                Debug.Log("found out startup");
            }
        }
    }
}