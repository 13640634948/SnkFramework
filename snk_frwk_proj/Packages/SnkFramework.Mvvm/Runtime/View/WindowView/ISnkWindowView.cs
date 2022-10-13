using SnkFramework.Mvvm.Core.ViewModel;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkWindowView : ISnkUIPage
        {
            public ISnkAnimation mActivationAnimation { get; set; }
            public ISnkAnimation mPassivationAnimation { get; set; }
        }

        public interface ISnkWindowView<TViewOwner, TViewModel> : ISnkWindowView, ISnkUIPage<TViewOwner, TViewModel>
            where TViewOwner : class, ISnkViewOwner
            where TViewModel : class, ISnkViewModel
        {

        }
    }
}