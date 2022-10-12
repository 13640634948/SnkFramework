using System;
using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IUILocator
    {
        public TView LoadView<TView>() where TView : class, ISnkView, new();

        public IEnumerator LoadViewAsync<TView>(Action<TView> callback) where TView : class, ISnkView, new();

        public TWindow LoadWindow<TWindow>(ISnkUILayer uiLayer) where TWindow : class, ISnkWindow, new();

        public IEnumerator LoadWindowAsync<TWindow>(ISnkUILayer uiLayer, Action<TWindow> callback) where TWindow : class, ISnkWindow, new();
    }
}