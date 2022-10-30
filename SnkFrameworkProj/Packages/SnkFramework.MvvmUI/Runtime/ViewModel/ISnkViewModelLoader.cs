using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelLoader
        {
            ISnkViewModel LoadViewModel(SnkViewModelRequest request, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null);

            ISnkViewModel LoadViewModel<TParameter>(SnkViewModelRequest request, TParameter param,
                ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);

            ISnkViewModel ReloadViewModel(ISnkViewModel viewModel, SnkViewModelRequest request, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null);

            ISnkViewModel ReloadViewModel<TParameter>(ISnkViewModel<TParameter> viewModel, TParameter param,
                SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);
        }
    }
}