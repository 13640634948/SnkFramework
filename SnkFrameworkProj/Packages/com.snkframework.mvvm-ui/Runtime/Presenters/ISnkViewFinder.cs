using System;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkViewFinder
        {
            Type GetViewType(Type viewModelType);
            Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel;
        }
    }
}