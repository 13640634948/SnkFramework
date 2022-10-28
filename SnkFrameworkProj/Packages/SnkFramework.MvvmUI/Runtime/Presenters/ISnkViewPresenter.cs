using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkViewPresenter
        {
            Task<bool> Show(SnkViewModelRequest request);

            //Task<bool> ChangePresentation(MvxPresentationHint hint);

            //void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : MvxPresentationHint;

            Task<bool> Close(ISnkViewModel viewModel);
        }
    }
}