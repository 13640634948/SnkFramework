using Codice.CM.SEIDInfo;

namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public interface ISnkViewModelCreator
    {
        public ISnkViewModel CreateViewModel(System.Type viewModelType);
        public TViewModel CreateViewModel<TViewModel>() where TViewModel : class, ISnkViewModel, new();
    }
}