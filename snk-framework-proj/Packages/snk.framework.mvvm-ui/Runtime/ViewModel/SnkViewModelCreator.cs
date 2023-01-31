using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewModelCreator : ISnkViewModelCreator
        {
            public ISnkViewModel CreateViewModel(Type viewModelType)
                => Activator.CreateInstance(viewModelType) as ISnkViewModel;

            public TViewModel CreateViewModel<TViewModel>() where TViewModel : class, ISnkViewModel, new()
                => new TViewModel();
        }
    }
}