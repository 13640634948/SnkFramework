using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loxodon.Framework.Interactivity;
using SnkFramework.Mvvm.Core;
using SnkFramework.Mvvm.Core.View;
using SnkFramework.Mvvm.Runtime.UGUI;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SnkFramework.Mvvm;

namespace SampleDevelop.Mvvm
{
    public class WindowDemo : MonoBehaviour, ISnkSetupMonitor
    {
        private List<LoginWindow> loginWindowList = new List<LoginWindow>();
        private UGUISnkMvvmManager _uguiSnkMvvmMgr => SnkIoCProvider.Instance.Resolve<ISnkMvvmManager>() as UGUISnkMvvmManager;
        public bool mIgnoreAnimation;
        
        public async Task InitializationComplete()
            => await UniTask.WaitUntil(()=>_completed );
        
        private bool _completed = false;
        
        private void Awake()
        {
            SnkMvvmSetup.RegisterSetupType<SnkDemoMvvmSetup>();
            var setupSingleton = SnkDemoSetupSingleton.EnsureSingletonAvailable(SynchronizationContext.Current);
            setupSingleton.PlatformSetup<SnkDemoMvvmSetup>().StateChanged += onSetupStateChanged;
            setupSingleton.InitializeAndMonitor(this);
        }

        private void onSetupStateChanged(object sender, SnkMvvmSetupStateEventArgs args)
        {
            Debug.Log("SetupState:" + args.SetupState);
        }


        private void Start()
        {
            _completed = true;
        }


        void Update()
        {
            foreach (var window in loginWindowList)
            {
                if (window != null)
                    window.mViewModel.Tip = Time.frameCount.ToString();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (this.loginWindowList.Count > 0)
                {
                    int index = this.loginWindowList.Count - 1;
                    this.loginWindowList[index].Dismiss(mIgnoreAnimation);
                    this.loginWindowList.RemoveAt(index);
                }
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (this.loginWindowList.Count > 0)
                {
                    int index = this.loginWindowList.Count - 1;
                    (this.loginWindowList[index].mViewModel.mInteractionFinished as InteractionRequest)?.Raise();
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                ISnkMvvmCoroutineExecutor executor = SnkIoCProvider.Instance.Resolve<ISnkMvvmCoroutineExecutor>();
                executor.RunOnCoroutineNoReturn(LoadWindowAsync());
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var window = _uguiSnkMvvmMgr.OpenWindow<LoginWindow>("normal");
                ISnkAnimation[] anims =
                {
                    new AlphaAnimation
                    {
                        AnimType = ANIM_TYPE.enter_anim,
                        from = 0.0f,
                        to = 1.0f,
                        duration = 1.0f
                    },
                    new AlphaAnimation
                    {
                        AnimType = ANIM_TYPE.exit_anim,
                        from = 1.0f,
                        to = 0.0f,
                        duration = 1.0f
                    }
                };

                foreach (var anim in anims)
                    anim.Initialize(window);

                window.Show(mIgnoreAnimation);
                this.loginWindowList.Add(window);
            }
        }

        private IEnumerator LoadWindowAsync()
        {
            yield return _uguiSnkMvvmMgr.OpenWindowAsync<LoginWindow>("normal", window =>
            {
                window.Show(mIgnoreAnimation);
                this.loginWindowList.Add(window);
            });
        }

        private void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit");
            foreach (var window in loginWindowList)
            {
                window.Dismiss(true);
            }

            this.loginWindowList.Clear();
            this.loginWindowList = null;
        }

    }
}