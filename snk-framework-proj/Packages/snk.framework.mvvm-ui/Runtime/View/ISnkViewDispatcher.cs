using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkViewDispatcher
        {
            Task ExecuteOnUIThreadAsync(Func<Task> action, bool maskExceptions = true);

            Task<bool> ShowViewModel(SnkViewModelRequest request);

            Task<bool> ChangePresentation(SnkPresentationHint hint);

            Task<bool> HideViewModel(ISnkViewModel viewModel);
        }
    }
}