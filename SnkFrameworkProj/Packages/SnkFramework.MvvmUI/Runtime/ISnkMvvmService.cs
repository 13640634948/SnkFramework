using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm
{
    public interface ISnkMvvmService
    {
    }

    public interface ISnkMvvmNavigation
    {
        //记录历史信息ViewHistory
    }

    public class SnkMvvmService : SnkContainer<ISnkLayer>, ISnkMvvmService
    {
        private ISnkMvvmNavigation _navigation;
        public ISnkViewDispatcher _viewDispatcher;

        public ISnkViewModelLoader _viewModelLoader;

        public async Task Navigate(SnkViewModelRequest request, ISnkViewModel viewModel)
        {
            await this._viewDispatcher.OpenViewModel(request).ConfigureAwait(false);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);
        }

        public async Task<TViewModel> OpenWindow<TViewModel>(ISnkBundle presentationBundle = null)
        where TViewModel : class, ISnkViewModel
        {
            var request = new SnkViewModelInstanceRequest(typeof(TViewModel))
            {
                PresentationBundle = presentationBundle
            };
            var viewModel = _viewModelLoader.LoadViewModel(request, null);
            request.ViewModelInstance = viewModel;
            
            await this._viewDispatcher.OpenViewModel(request).ConfigureAwait(false);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);
            return viewModel as TViewModel;
        }
    }
}