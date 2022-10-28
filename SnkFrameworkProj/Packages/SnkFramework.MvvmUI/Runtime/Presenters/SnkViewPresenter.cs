using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public abstract class SnkViewPresenter : ISnkViewPresenter
        {
            public abstract Task<bool> Show(SnkViewModelRequest request);
            public abstract Task<bool> Close(ISnkViewModel viewModel);
        }
    }
}