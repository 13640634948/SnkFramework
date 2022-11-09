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
        [RequireComponent(typeof(CanvasScaler))]
        public abstract class SnkUILayer : SnkUIBehaviour, ISnkLayer
        {
            private CanvasScaler _canvasScaler;
            public CanvasScaler canvasScaler => _canvasScaler ??= GetComponent<CanvasScaler>();

            public virtual string LayerName => this.GetType().Name;


            public List<SnkWindow> WindowList = new List<SnkWindow>();


            public void AddChild(SnkWindow window)
            {
                WindowList.Add(window);
                window.rectTransform.SetParent(rectTransform);
                window.rectTransform.Identity();
                window.canvas.overrideSorting = true;
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