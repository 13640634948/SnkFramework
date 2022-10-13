using SnkFramework.Mvvm.Core.ViewModel;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkUIPage : ISnkUIView
        {
        }

        public interface ISnkUIPage<TViewOwner, TViewModel> : ISnkUIPage, ISnkUIView<TViewOwner, TViewModel>
            where TViewOwner : class, ISnkViewOwner
            where TViewModel : class, ISnkViewModel
        {
        }
    }
}