using System;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public interface ISnkViewModelLocator
    { 
        ISnkViewModel Load(
            Type viewModelType,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null);
 
        ISnkViewModel<TParameter> Load<TParameter>(
            Type viewModelType,
            TParameter param,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null)
            where TParameter : notnull;

 
        ISnkViewModel Reload(
            ISnkViewModel viewModel,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null);

 
        ISnkViewModel<TParameter> Reload<TParameter>(
            ISnkViewModel<TParameter> viewModel,
            TParameter param,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null)
            where TParameter : notnull;
    }
}