using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public partial class SnkViewPresenter
    {
        protected virtual async Task<bool> ShowWindow(ISnkPresentationAttribute attribute, SnkViewModelRequest request)
            => default;

        protected virtual async Task<bool> CloseWindow(ISnkViewModel viewModel, ISnkPresentationAttribute attribute)
            => default;
    }
}