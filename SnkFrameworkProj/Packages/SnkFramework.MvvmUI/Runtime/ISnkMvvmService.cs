using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm
{
    public interface ISnkMvvmService
    {
    }

    public interface ISnkMvvmNavigation
    {
        //记录历史信息ViewHistory
    }

    public class SnkBehaviourOwner : MonoBehaviour
    {
    }

    public class SnkMvvmService : SnkContainer<ISnkLayer>, ISnkMvvmService
    {
        private ISnkMvvmNavigation _navigation;
        private ISnkViewDispatcher _viewDispatcher { get; }

        public async Task Navigate(SnkViewModelRequest request, ISnkViewModel viewModel)
        {
            await this._viewDispatcher.ShowViewModel(request).ConfigureAwait(false);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);
        }

        public async Task<TViewModel> OpenWindow<TView, TViewModel>()
            where TView : class, ISnkWindow
            where TViewModel : class, ISnkViewModel
        {
            SnkViewModelRequest request = new SnkViewModelRequest();
            request.ViewType = typeof(TView);
            request.ViewModelType = typeof(TViewModel);
            return default;
        }
        
    }
}