using System.Collections;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
        public abstract partial class SnkWindow : SnkView, ISnkWindow
        {
            private Canvas _canvas;
            public Canvas Canvas => _canvas ??= GetComponent<Canvas>();

            private CanvasGroup _canvasGroup;
            public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

            public override void Create(ISnkBundle bundle)
            {
            }

            public virtual SnkTransitionOperation Show(bool animated)
            {
                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onShowTransitioning(operation);
                if (routine != null && animated)
                    StartCoroutine(routine);
                return operation;
            }

            public virtual SnkTransitionOperation Hide(bool animated)
            {
                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onHideTransitioning(operation);
                if (routine != null && animated)
                    StartCoroutine(routine);
                return operation;
            }

            /// <summary>
            /// 显示动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onShowTransitioning(SnkTransitionOperation operation) => default;
            
            /// <summary>
            /// 隐藏动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onHideTransitioning(SnkTransitionOperation operation) => default;
        }
    }
}