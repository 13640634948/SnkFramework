using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public delegate void UILayoutOverride(RectTransform rectTransform);

    public interface IUILayer
    {
        public bool mActivated { get; }
        public IWindow mCurrent { get; }
        public int IndexOf(IWindow window);

        public bool Remove(IWindow window);
        public void Add(IWindow window);

        public ITransition Show(ISnkWindowControllable window);

        public ITransition Hide(ISnkWindowControllable window);

        public ITransition Dismiss(ISnkWindowControllable window);
    }
}