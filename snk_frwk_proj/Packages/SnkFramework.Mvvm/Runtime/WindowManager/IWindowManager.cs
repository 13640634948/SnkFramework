using System;
using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base

{
    public interface IWindowManager
    {
        public ISnkUILayer GetLayer(string layerName);
        public ISnkUILayer CreateLayer(string layerName);
        public bool TryGetLayer(string layerName, out ISnkUILayer uiLayer);

        public TWindow OpenWindow<TWindow>(string layerName, bool ignoreAnimation = false)
            where TWindow : class, ISnkWindow, new();

        public IEnumerator OpenWindowAsync<TWindow>(string layerName, Action<TWindow> callback,
            bool gnoreAnimation = false)
            where TWindow : class, ISnkWindow, new();
    }
}