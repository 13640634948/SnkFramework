using System.Threading;
using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Runtime.View
{
    public class SnkViewDispatcher : SnkMainThreadAsyncDispatcher, ISnkViewDispatcher
    {
        private readonly ISnkViewPresenter _presenter;

        public SnkViewDispatcher(ISnkViewPresenter presenter) : base(SynchronizationContext.Current)
        {
            _presenter = presenter;
        }

        public async Task ShowViewModel(SnkViewModelRequest request)
            => await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
    }
}