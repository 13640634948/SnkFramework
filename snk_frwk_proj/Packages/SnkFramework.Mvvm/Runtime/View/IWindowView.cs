using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;

namespace SnkFramework.Mvvm.View
{
    public interface IWindowView : IUIContainer, IView
    {
        public IAnimation mActivationAnimation { get; set; }
        public IAnimation mPassivationAnimation { get; set; }
    }

    public interface IWindowView<TViewModel> : IWindowView, IView<TViewModel>
        where TViewModel : class, IViewModel
    {
        
    }
}