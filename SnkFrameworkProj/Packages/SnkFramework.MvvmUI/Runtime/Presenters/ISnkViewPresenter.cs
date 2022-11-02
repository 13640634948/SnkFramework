using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkViewPresenter
        {
            Task<bool> Open(SnkViewModelRequest request);

            Task<bool> ChangePresentation(SnkPresentationHint hint);

            void AddPresentationHintHandler<THint>(System.Func<THint, Task<bool>> action) where THint : SnkPresentationHint;

            Task<bool> Close(ISnkViewModel viewModel);
        }
    }
}