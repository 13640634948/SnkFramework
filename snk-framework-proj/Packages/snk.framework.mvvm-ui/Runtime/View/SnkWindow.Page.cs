using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Layer;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract partial class SnkWindow
        {
            public ISnkLayer Layer { get; protected set; }

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

            public void AddPage(ISnkPage page)
            {
            }

            public TViewModel AddPage<TViewModel>()
            {
                return default;
            }

            public Task AddPageAsync<TViewModel>() => default;
        }
    }
}