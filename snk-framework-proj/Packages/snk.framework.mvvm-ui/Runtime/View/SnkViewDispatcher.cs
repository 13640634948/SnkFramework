using System.Threading;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public class SnkViewDispatcher : SnkMainThreadAsyncDispatcher, ISnkViewDispatcher
        {
            private readonly ISnkViewPresenter _presenter;

            public SnkViewDispatcher(ISnkViewPresenter presenter) : base(SynchronizationContext.Current)
            {
                _presenter = presenter;
            }

            public async Task<bool> ShowViewModel(SnkViewModelRequest request)
            {
                await ExecuteOnMainThreadAsync(() => _presenter.Open(request));
                return true;
            }

            public async Task<bool> HideViewModel(ISnkViewModel viewModel)
            {
                await ExecuteOnMainThreadAsync(() => _presenter.Close(viewModel));
                return true;
            }

            public async Task<bool> ChangePresentation(SnkPresentationHint hint)
            {
                await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
                return true;
            }
        }
    }
}