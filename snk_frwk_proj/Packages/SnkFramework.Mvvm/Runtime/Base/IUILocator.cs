using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IUILocator
    {
        public T LoadWindow<T>(string windowPath, IUILayer uiLayer)
            where T : class, IWindow, new();

        public ILoadOperation LoadWindowAsync(string windowPath, IUILayer uiLayer);
    }
}