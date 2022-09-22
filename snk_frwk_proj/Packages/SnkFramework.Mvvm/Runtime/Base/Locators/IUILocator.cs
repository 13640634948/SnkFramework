using System;
using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IUILocator
    {
        public TView LoadView<TView>() where TView : class, IView, new();

        public IEnumerator LoadViewAsync<TView>(Action<TView> callback) where TView : class, IView, new();

        public TWindow LoadWindow<TWindow>(IUILayer uiLayer) where TWindow : class, IWindow, new();

        public IEnumerator LoadWindowAsync<TWindow>(IUILayer uiLayer, Action<TWindow> callback) where TWindow : class, IWindow, new();
    }
}