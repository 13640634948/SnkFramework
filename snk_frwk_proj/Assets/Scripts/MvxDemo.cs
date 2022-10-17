using System.Threading;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.UnityEngine.Core;
using MvvmCross.UnityEngine.ViewModels;
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
            RegisterAppStart<DemoViewModel>();
        }
    }


    public class DemoSetup : MvxUnitySetup<DemoApp>
    {
    }

    public class DemoViewModel : MvxUnityViewModel
    {
        public DemoViewModel()
        {
            UnityEngine.Debug.Log("DemoViewModel.ctor");
        }
    }

    public class DemoView : MvxUGUIView<DemoViewModel>
    {
        public DemoView()
        {
            UnityEngine.Debug.Log("DemoView.ctor");
        }
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