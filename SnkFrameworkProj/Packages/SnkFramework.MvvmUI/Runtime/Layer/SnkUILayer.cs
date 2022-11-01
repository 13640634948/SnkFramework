using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public abstract class SnkUILayer : SnkUIBehaviour, ISnkLayer
    {
        public virtual string LayerName => this.GetType().Name;
        public void AddChild(SnkUIBehaviour windowUIBehaviour)
        {
            windowUIBehaviour.rectTransform.SetParent(rectTransform);
            //RectTransform rectTransform = windowUIBehaviour.transform as RectTransform;
            windowUIBehaviour.SetRectTransformIdentity();
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