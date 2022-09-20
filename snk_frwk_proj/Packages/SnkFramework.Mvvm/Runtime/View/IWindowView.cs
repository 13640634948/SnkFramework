using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;

namespace SnkFramework.Mvvm.View
{
    public interface IWindowView : IUIContainer, IView
    {
        public IAnimation ActivationAnimation { get; }
        public IAnimation PassivationAnimation { get; }
    }

    public interface IWindowView<TViewModel> : IWindowView, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        
    }
}