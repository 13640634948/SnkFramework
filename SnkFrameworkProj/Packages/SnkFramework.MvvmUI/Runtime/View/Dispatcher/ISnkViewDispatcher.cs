using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkViewDispatcher
    {
        public Task ShowViewModel(SnkViewModelRequest request);
    }
}