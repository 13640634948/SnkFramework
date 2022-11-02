using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm
{
    public interface ISnkMvvmService
    {
    }
    
    

    public class SnkMvvmService : SnkContainer<ISnkLayer>, ISnkMvvmService
    {
        private ISnkViewDispatcher _viewDispatcher;

        private ISnkViewModelLoader _viewModelLoader;

        public SnkMvvmService(ISnkViewDispatcher viewDispatcher, ISnkViewModelLoader viewModelLoader)
        {
            this._viewDispatcher = viewDispatcher;
            this._viewModelLoader = viewModelLoader;
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
            
            await this._viewDispatcher.ShowViewModel(request).ConfigureAwait(false);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);
            return viewModel as TViewModel;
        }
        
        public virtual async Task<bool> Close(ISnkViewModel viewModel)
        {
            var hit = new SnkClosePresentationHint(viewModel);
            return await this._viewDispatcher.ChangePresentation(hit).ConfigureAwait(false);
        }
    }
}