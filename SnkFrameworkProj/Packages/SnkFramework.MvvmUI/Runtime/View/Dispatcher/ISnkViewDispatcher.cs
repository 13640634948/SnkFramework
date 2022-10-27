using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkViewDispatcher
    {
        public Task ShowViewModel(SnkViewModelRequest request);
    }
}