using SnkFramework.Mvvm.Runtime.Layer;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract partial class SnkWindow
        {
            public ISnkView Current { get; }
            public ISnkView NavigatorPrev { get; }
            public ISnkView NavigatorNext { get; }

            public void Add(ISnkPage target)
            {
                
            }

            public bool Remove(ISnkPage target)
            {
                return true;
            }

            public SnkWindowState WindowState { get; }
            public ISnkLayer Layer { get; }


            public void AddPage(ISnkPage page)
            {
            }

            public TViewModel AddPage<TViewModel>()
            {
                return default;
            }

            public SnkTransitionOperation AddPageAsync<TViewModel>() => default;
        }
    }
}