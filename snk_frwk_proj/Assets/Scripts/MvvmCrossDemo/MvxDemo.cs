using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Core;
using MvvmCross.Unity.Core;
using MvvmCross.ViewModels;
using UnityEngine;

namespace MvvmCross.Demo
{
    public class MvxDemo : MonoBehaviour, IMvxSetupMonitor
    {
        protected virtual void Awake()
        {
            this.RegisterSetupType<DemoSetup>();
            MvxUnitySetupSingleton.EnsureSingletonAvailable().InitializeAndMonitor(this);
        }

        protected virtual void OnApplicationQuit()
        {
            MvxSingleton.ClearAllSingletons();
        }

        async public Task InitializationComplete()
        {
            await runAppStart(true);
        }

        protected Task runAppStart(bool async)
        {
            return Task.Run(() =>
            {
                if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                {
                    if (async)
                        startup.StartAsync();
                    else
                        startup.Start();
                }
            });
        }
    }
}