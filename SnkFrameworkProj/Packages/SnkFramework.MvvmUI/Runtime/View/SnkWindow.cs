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
            }

            public virtual SnkTransitionOperation Activate(bool animated)
            {
                this.canvasGroup.alpha = 0;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(onActivateTransitioning(operation));
                return operation;
            }

            public virtual SnkTransitionOperation Passivate(bool animated)
            {
                this.canvasGroup.alpha = 1;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(onPassivateTransitioning(operation));
                return operation;
            }

            public virtual SnkTransitionOperation Show(bool animated)
            {
                this.canvasGroup.alpha = 1;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(onPassivateTransitioning(operation));
                return operation;
            }


            public virtual SnkTransitionOperation Hide(bool animated)
            {
                this.canvasGroup.alpha = 1;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                StartCoroutine(onPassivateTransitioning(operation));
                return operation;
            }

            /// <summary>
            /// 激活动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onActivateTransitioning(SnkTransitionOperation operation) => default;

            /// <summary>
            /// 钝化动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onPassivateTransitioning(SnkTransitionOperation operation) => default;
            protected virtual IEnumerator onShowTransitioning(SnkTransitionOperation operation) => default;
            protected virtual IEnumerator onHideTransitioning(SnkTransitionOperation operation) => default;


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


            public void AddPage(ISnkPage page)
            {
                throw new System.NotImplementedException();
            }

            public TViewModel AddPage<TViewModel>()
            {
                throw new System.NotImplementedException();
            }

            public SnkTransitionOperation AddPageAsync<TViewModel>() => default;
        }
    }
}