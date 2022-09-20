using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UILocator : IUILocator
    {
        public abstract TView LoadView<TView>() where TView : class, IView, new();

        public abstract UILoadResult<TView> LoadViewAsync<TView>() where TView : class, IView, new();

        public abstract TWindow LoadWindow<TWindow>(IUILayer uiLayer) where TWindow : class, IWindow, new();
        public abstract TWindow LoadWindow<TWindow>(string layerName) where TWindow : class, IWindow, new();

        public abstract UILoadResult<TWindow> LoadWindowAsync<TWindow>(IUILayer uiLayer)
            where TWindow : class, IWindow, new();
    }
}