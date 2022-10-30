namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public interface ISnkViewModelCreator
    {
        public ISnkViewModel CreateViewModel(System.Type viewModelType);
    }
}