using System.Collections;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public delegate void UILayoutOverride(RectTransform rectTransform);

    public interface IUILayer
    {
        public bool Remove(IWindow window);
        public void Add(IWindow window);
        ITransition Show(IWindow window);
    }
}