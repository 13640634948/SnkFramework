using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelLoader : ISnkViewModelLocator
        {
            ISnkViewModel LoadViewModel(SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);

            ISnkViewModel ReloadViewModel(ISnkViewModel viewModel, SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);

        }
    }
}