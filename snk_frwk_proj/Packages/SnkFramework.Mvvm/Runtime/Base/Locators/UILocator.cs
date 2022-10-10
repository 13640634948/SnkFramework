using System;
using System.Collections;
using SampleDevelop.Test;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UILocator : IUILocator
    {
        public abstract TView LoadView<TView>() where TView : class, ISnkView, new();
        public abstract IEnumerator LoadViewAsync<TView>(Action<TView> callback) where TView : class, ISnkView, new();
        public abstract TWindow LoadWindow<TWindow>(ISnkUILayer uiLayer) where TWindow : class, ISnkWindow, new();
        public abstract IEnumerator LoadWindowAsync<TWindow>(ISnkUILayer uiLayer, Action<TWindow> callback) where TWindow : class, ISnkWindow, new();
    }
}