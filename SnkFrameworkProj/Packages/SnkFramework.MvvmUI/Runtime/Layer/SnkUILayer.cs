using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public abstract class SnkTransition : ISnkTransition
    {
        protected ISnkWindow window;
        public SnkTransition(ISnkWindow window)
        {
            this.window = window;
        }

        public Task<bool> DoTransitionTask()
        {
            return Task.FromResult(true);
        }
    }

    public class ShowWindowTransition : SnkTransition
    {
        public ShowWindowTransition(ISnkWindow window) : base(window)
        {
        }
    }
    public class HideWindowTransition : SnkTransition
    {
        private bool _dismiss;
        public HideWindowTransition(ISnkWindow window, bool dismiss) : base(window)
        {
            this._dismiss = dismiss;
        }
    }
    
    public interface ISnkTransitionController
    {
        public Task<bool>  Show(ISnkWindow window);
        public Task<bool> Hide(ISnkWindow window);
        public Task<bool> Dismiss(ISnkWindow window);
    }
    
    [RequireComponent(typeof(CanvasScaler))]
    public abstract class SnkUILayer : SnkUIBehaviour, ISnkLayer, ISnkTransitionController
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

        public Task<bool> Show(ISnkWindow window)
        {
            var transition = new ShowWindowTransition(window);
            return this.transitionExecute(transition);
        }

        public Task<bool> Hide(ISnkWindow window)
        {
            var transition = new HideWindowTransition(window, false);
            return this.transitionExecute(transition);
        }

        public Task<bool> Dismiss(ISnkWindow window)
        {
            var transition = new HideWindowTransition(window, true);
            return this.transitionExecute(transition);
        }

    }

    public class SnkUGUINormalLayer : SnkUILayer
    {
    }

    public class SnkUGUIDialogueLayer : SnkUILayer
    {
    }

    public class SnkUGUIGuideLayer : SnkUILayer
    {
    }

    public class SnkUGUITopLayer : SnkUILayer
    {
    }

    public class SnkUGUILoadingLayer : SnkUILayer
    {
    }

    public class SnkUGUISystemLayer : SnkUILayer
    {
    }
}