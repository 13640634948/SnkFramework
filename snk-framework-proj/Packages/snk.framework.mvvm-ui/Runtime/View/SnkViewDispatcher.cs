using System;
using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkViewDispatcher : ISnkViewDispatcher
        {
            private Lazy<ISnkViewPresenter> _presenter = new (()=> 
                SnkSingleton<ISnkIoCProvider>.Instance.Resolve<ISnkViewPresenter>());

            public abstract Task ExecuteOnUIThreadAsync(Func<Task> action, bool maskExceptions = true);

            public async Task<bool> ShowViewModel(SnkViewModelRequest request)
            {
                await ExecuteOnUIThreadAsync(() => _presenter.Value.Open(request));
                return true;
            }

            public async Task<bool> HideViewModel(ISnkViewModel viewModel)
            {
                await ExecuteOnUIThreadAsync(() => _presenter.Value.Close(viewModel));
                return true;
            }

            public async Task<bool> ChangePresentation(SnkPresentationHint hint)
            {
                await ExecuteOnUIThreadAsync(() => _presenter.Value.ChangePresentation(hint));
                return true;
            }
        }
    }
}