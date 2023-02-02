using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkViewLoader : ISnkViewLoader
        {
            public virtual async Task<SnkWindow> CreateView(SnkViewModelRequest request)
            {
                var viewType = GetViewType(request.ViewModelType);
                return await CreateView(viewType);
            }

            public abstract Task<SnkWindow> CreateView(Type viewType);

            public virtual bool UnloadView(SnkWindow window)
            {
                UnityEngine.Object.Destroy(window.gameObject);
                return true;
            }

            public abstract Type GetViewType(Type viewModelType);

            public virtual Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel
                => GetViewType(typeof(TViewModel));
        }
    }
}