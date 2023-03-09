using System;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelLocator : ISnkViewModelCreator
        {
            ISnkViewModel Load(Type viewModelType, ISnkBundle parameterValues, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);


            ISnkViewModel Reload(ISnkViewModel viewModel, ISnkBundle parameterValues, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null);
        }
    }
}