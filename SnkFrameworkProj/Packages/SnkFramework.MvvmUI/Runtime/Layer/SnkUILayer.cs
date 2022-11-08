using System.Collections.Generic;
using System.Threading.Tasks;
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
                window.SetRectTransformIdentity();
                window.canvas.overrideSorting = true;
            }

            public SnkWindow GetChild(int index)
            {
                return WindowList[index];
            }

            public virtual SnkTransitionOperation Open(ISnkWindow window)
            {
                Debug.Log("Layer.Open-Begin");
                var operation = window.Activate();
                Debug.Log("Layer.Open-End");
                return operation;
            }

            public virtual SnkTransitionOperation Close(ISnkWindow window)
            {
                return window.Passivate();
            }
        }
    }
}