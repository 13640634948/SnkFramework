using System.Threading;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.UnityEngine.Core;
using MvvmCross.UnityEngine.Views.UGUI;
using MvvmCross.ViewModels;
using UnityEngine;

namespace DefaultNamespace
{
    public class DemoApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<MvxViewModel>();
        }
    }


    public class DemoSetup : MvxUnitySetup<DemoApp>
    {
    }

    public class DemoViewModel : MvxViewModel
    {
    }

    public class DemoView : MvxUGUIView
    {
    }

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