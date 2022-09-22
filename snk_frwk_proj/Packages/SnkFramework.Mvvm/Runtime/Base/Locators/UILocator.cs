using System;
using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UILocator : IUILocator
    {
        public abstract TView LoadView<TView>() where TView : class, IView, new();
        public abstract IEnumerator LoadViewAsync<TView>(Action<TView> callback) where TView : class, IView, new();
        public abstract TWindow LoadWindow<TWindow>(IUILayer uiLayer) where TWindow : class, IWindow, new();
        public abstract IEnumerator LoadWindowAsync<TWindow>(IUILayer uiLayer, Action<TWindow> callback) where TWindow : class, IWindow, new();
    }
}