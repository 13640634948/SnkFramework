using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Core;
using MvvmCross.Unity.Core;
using MvvmCross.Unity.ViewModels;
using MvvmCross.ViewModels;
using UnityEngine;

namespace MvvmCross.Demo
{
    public class MvxDemo : MonoBehaviour, IMvxSetupMonitor
    {
        protected virtual void Awake()
        {
            this.RegisterSetupType<DemoSetup>();
            var singleton = MvxUnitySetupSingleton.EnsureSingletonAvailable();
            singleton.EnsureInitialized();
            singleton.InitializeAndMonitor(this);
        }

        protected virtual void OnApplicationQuit()
        {
            MvxSingleton.ClearAllSingletons();
        }

        public Task InitializationComplete()
        {
            MvxUnityViewModelParameter param = new DemoStartupViewModelParameter();
            return runAppStart(true, param);
        }

        protected Task runAppStart(bool async, MvxUnityViewModelParameter vmParameter = null )
        {
            return Task.Run(() =>
            {
                if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                {
                    if (async)
                        startup.StartAsync(vmParameter);
                    else
                        startup.Start(vmParameter);
                }
            });
        }
    }
}