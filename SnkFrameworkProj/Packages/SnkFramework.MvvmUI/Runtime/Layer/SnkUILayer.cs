using System.Collections.Generic;
using SnkFramework.Mvvm.Extensions;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkUINodeUnit
        {
            public Canvas Canvas { get; }
            public CanvasGroup CanvasGroup { get; }
        }

        [RequireComponent(typeof(Canvas), typeof(CanvasScaler),typeof(CanvasGroup) )]
        public abstract class SnkUILayer : SnkUIBehaviour, ISnkLayer
        {
            private Canvas _canvas;
            public Canvas Canvas => _canvas ??= GetComponent<Canvas>();
            
            private CanvasScaler _canvasScaler;
            public CanvasScaler CanvasScaler => _canvasScaler ??= GetComponent<CanvasScaler>();

            private CanvasGroup _canvasGroup;
            public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

            public virtual string LayerName => this.GetType().Name;

            public List<SnkWindow> WindowList = new List<SnkWindow>();

            public void AddChild(SnkWindow window)
            {
                WindowList.Add(window);
                window.rectTransform.SetParent(rectTransform);
                window.rectTransform.Identity();
                window.Canvas.overrideSorting = true;
            }

            public SnkWindow GetChild(int index)
            {
                return WindowList[index];
            }

            public virtual SnkTransitionOperation Open(ISnkWindow window)
            {
                bool animated = true;
                var operation = window.Activate(animated);
                return operation;
            }

            public virtual SnkTransitionOperation Close(ISnkWindow window)
            {
                bool animated = true;
                return window.Passivate(animated);
            }
        }
    }
}