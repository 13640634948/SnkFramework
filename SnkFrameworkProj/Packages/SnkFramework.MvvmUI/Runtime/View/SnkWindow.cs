using System.Collections;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkWindow : SnkUIBehaviour, ISnkWindow
        {
            public ISnkViewModel ViewModel { get; set; }
            public bool Visibility { get; set; }
            public bool Interactable { get; set; }
            public bool Activated { get; set; }

            public void Create(ISnkBundle bundle)
            {
                //this.canvasGroup.alpha = 0;
            }

            public SnkTransitionOperation Activate()
            {
                this.canvasGroup.alpha = 0;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(ActivateAnimation(operation));
                return operation;
            }

            public SnkTransitionOperation Passivate() 
            {
                this.canvasGroup.alpha = 1;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(PassivateAnimation(operation));
                return operation;
            }
            
            /// <summary>
            /// 激活动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator ActivateAnimation(SnkTransitionOperation operation)
            {
                float currTime = Time.realtimeSinceStartup;
                float progress = 0;
                while (progress <1.0f)
                {
                    var dt = Time.realtimeSinceStartup - currTime;
                    progress = dt / 3.0f;
                    this.canvasGroup.alpha = progress;
                    yield return null;
                }

                this.canvasGroup.alpha = 1.0f;
                operation.IsDone = true;
                operation.onCompleted?.Invoke();
            }

            /// <summary>
            /// 钝化动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator PassivateAnimation(SnkTransitionOperation operation)
            {
                float currTime = Time.realtimeSinceStartup;
                float progress = 0;
                while (progress <1.0f)
                {
                    var dt = Time.realtimeSinceStartup - currTime;
                    progress = dt / 3.0f;
                    this.canvasGroup.alpha = 1-progress;
                    yield return null;
                }

                this.canvasGroup.alpha = 0.0f;
                operation.IsDone = true;
                operation.onCompleted?.Invoke();
            }

            public ISnkView Current { get; }
            public ISnkView NavigatorPrev { get; }
            public ISnkView NavigatorNext { get; }

            public void Add(ISnkPage target)
            {
                throw new System.NotImplementedException();
            }

            public bool Remove(ISnkPage target)
            {
                throw new System.NotImplementedException();
            }

            public SnkWindowState WindowState { get; }
            public ISnkLayer Layer { get; }

            public SnkTransitionOperation  Show(bool animated) => default;


            public SnkTransitionOperation  Hide(bool animated) => default;


            public void AddPage(ISnkPage page)
            {
                throw new System.NotImplementedException();
            }

            public TViewModel AddPage<TViewModel>()
            {
                throw new System.NotImplementedException();
            }

            public SnkTransitionOperation  AddPageAsync<TViewModel>() => default;
        }
    }
}