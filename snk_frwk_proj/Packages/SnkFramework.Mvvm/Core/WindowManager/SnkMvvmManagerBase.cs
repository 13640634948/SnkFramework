using System;
using System.Collections.Generic;
using System.Collections;
using SnkFramework.Mvvm.Core.View;

namespace SnkFramework.Mvvm.Core
{
    public abstract class SnkMvvmManagerBase<TUILayer> : ISnkMvvmManager
        where TUILayer : class, ISnkUILayer, new()
    {
        protected Dictionary<string, TUILayer> layerDict;
        protected ISnkMvvmCoroutineExecutor coroutineExecutor;
        protected ISnkTransitionExecutor transitionExecutor;

        public abstract ISnkUILayer CreateLayer(string layerName);

        public SnkMvvmManagerBase(ISnkMvvmCoroutineExecutor coroutineExecutor)
        {
            layerDict = new Dictionary<string, TUILayer>();

            this.coroutineExecutor = coroutineExecutor;
            this.transitionExecutor = createTransitionExecutor(coroutineExecutor);
        }

        protected virtual ISnkTransitionExecutor createTransitionExecutor(ISnkMvvmCoroutineExecutor coroutineExecutor)
            => new SnkTransitionPopupExecutor(coroutineExecutor);

        public virtual ISnkUILayer GetLayer(string layerName)
        {
            if (TryGetLayer(layerName, out ISnkUILayer layer) == false)
                return null;
            return layer;
        }

        public virtual bool TryGetLayer(string layerName, out ISnkUILayer uiLayer)
        {
            bool result = this.layerDict.TryGetValue(layerName, out TUILayer layer);
            uiLayer = layer;
            return result;
        }

        public virtual TWindow OpenWindow<TWindow>(string layerName, bool ignoreAnimation = false)
            where TWindow : class, ISnkWindow, new()
        {
            var layer = this.GetLayer(layerName);
            var window = new TWindow();
            layer.Add(window);
            window.LoadViewOwner();
            window.Create();
            window.Show(ignoreAnimation);
            return window;
        }

        public virtual IEnumerator OpenWindowAsync<TWindow>(string layerName, Action<TWindow> callback,
            bool ignoreAnimation = false)
            where TWindow : class, ISnkWindow, new()
        {
            var layer = this.GetLayer(layerName);
            var window = new TWindow();
            layer.Add(window);
            yield return window.LoadViewOwnerAsync();
            window.Create();
            window.Show(ignoreAnimation);
            callback?.Invoke(window);
        }
    }
}