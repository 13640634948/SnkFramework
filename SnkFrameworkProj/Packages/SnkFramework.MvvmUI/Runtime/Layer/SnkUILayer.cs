using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    [RequireComponent(typeof(CanvasScaler))]
    public abstract class SnkUILayer : SnkUIBehaviour, ISnkLayer
    {
        private CanvasScaler _canvasScaler;
        public CanvasScaler canvasScaler => _canvasScaler ??= GetComponent<CanvasScaler>();

        public virtual string LayerName => this.GetType().Name;

        public void AddChild(SnkUIBehaviour windowUIBehaviour)
        {
            windowUIBehaviour.rectTransform.SetParent(rectTransform);
            windowUIBehaviour.SetRectTransformIdentity();
            windowUIBehaviour.canvas.overrideSorting = true;
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