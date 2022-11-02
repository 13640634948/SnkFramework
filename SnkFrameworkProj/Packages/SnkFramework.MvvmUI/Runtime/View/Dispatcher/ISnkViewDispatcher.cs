using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkViewDispatcher
    {
        Task<bool> ShowViewModel(SnkViewModelRequest request);

        Task<bool> ChangePresentation(SnkPresentationHint hint);
    }
}