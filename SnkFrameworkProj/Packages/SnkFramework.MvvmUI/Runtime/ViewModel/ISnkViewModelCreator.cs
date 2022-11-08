namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelCreator
        {
            public ISnkViewModel CreateViewModel(System.Type viewModelType);
            public TViewModel CreateViewModel<TViewModel>() where TViewModel : class, ISnkViewModel, new();
        }
    }
}