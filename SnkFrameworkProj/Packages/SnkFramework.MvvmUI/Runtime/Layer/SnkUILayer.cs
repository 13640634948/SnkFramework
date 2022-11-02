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


            protected virtual Task<bool> transitionExecute(ISnkTransition transition)
            {
                return transition.DoTransitionTask();
            }

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

            public Task<bool> Open(ISnkWindow window)
            {
                var transition = new ShowWindowTransition(window);
                return this.transitionExecute(transition);
            }


            public Task<bool> Close(ISnkWindow window)
            {
                var transition = new HideWindowTransition(window, true);
                return this.transitionExecute(transition);
            }
        }

      
    }
}