using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkViewLoader : ISnkViewLoader
        {
            private ISnkViewFinder _viewFinder;

            public SnkViewLoader(ISnkViewFinder viewFinder)
            {
                _viewFinder = viewFinder;
            }

            public virtual async Task<SnkWindow> CreateView(SnkViewModelRequest request)
            {
                var viewType = _viewFinder.GetViewType(request.ViewModelType);
                return await CreateView(viewType);
            }

            public abstract Task<SnkWindow> CreateView(Type viewType);

            public virtual bool UnloadView(SnkWindow window)
            {
                UnityEngine.Object.Destroy(window.gameObject);
                return true;
            }
        }
    }
}