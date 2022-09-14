using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IUILoadResult
    {
        public object mResult { get; set; }
    }

    public interface IUILoadResult<T> : IUILoadResult
    {
        public new T mResult { get; set; }
    }

    public class UILoadResult : IUILoadResult
    {
        public object mResult { get; set; }
    }

    public class UILoadResult<T> : UILoadResult, IUILoadResult<T>
    {
        public new T mResult { get; set; }
    }


    public interface IUILocator
    {
        public TView LoadView<TView>() where TView : class, IView, new();

        public UILoadResult<TView> LoadViewAsync<TView>() where TView : class, IView, new();

        public TWindow LoadWindow<TWindow>(IUILayer uiLayer) where TWindow : class, IWindow, new();

        public UILoadResult<TWindow> LoadWindowAsync<TWindow>(IUILayer uiLayer) where TWindow : class, IWindow, new();
    }
}