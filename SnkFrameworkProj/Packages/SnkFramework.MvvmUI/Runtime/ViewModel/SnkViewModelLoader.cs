using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public class SnkViewModelLoader : ISnkViewModelLoader
    {
        protected ISnkViewModelLocator _viewModelLocator;
        public SnkViewModelLoader(ISnkViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;
        }

        public ISnkViewModel LoadViewModel(SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
        {
            try
            {
                return _viewModelLocator.Load(request.ViewModelType!, request.ParameterBundle, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ISnkViewModel LoadViewModel<TParameter>(SnkViewModelRequest request, TParameter param, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
        {

            try
            {
                return _viewModelLocator.Load(request.ViewModelType!, param, request.ParameterBundle, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ISnkViewModel ReloadViewModel(ISnkViewModel viewModel, SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
        {
            try
            {
                viewModel = _viewModelLocator.Reload(viewModel, request.ParameterBundle, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }

            return viewModel;
        }

        public ISnkViewModel ReloadViewModel<TParameter>(ISnkViewModel<TParameter> viewModel, TParameter param, SnkViewModelRequest request, ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
        {

            try
            {
                return _viewModelLocator.Reload(viewModel, param, request.ParameterBundle, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }
    }
}