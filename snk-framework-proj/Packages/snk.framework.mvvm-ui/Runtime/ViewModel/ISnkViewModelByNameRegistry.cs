using System;
using System.Reflection;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelByNameRegistry
        {
            void Add(Type viewModelType);

            void Add<TViewModel>() where TViewModel : ISnkViewModel;

            void AddAll(Assembly assembly);
        }
    }
}