using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewModelLoader : ISnkViewModelLoader
        {
            protected ISnkViewModelLocator _viewModelLocator;

            public SnkViewModelLoader(ISnkViewModelLocator viewModelLocator)
            {
                _viewModelLocator = viewModelLocator;
            }

            public ISnkViewModel LoadViewModel(SnkViewModelRequest request, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null)
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


            public ISnkViewModel ReloadViewModel(ISnkViewModel viewModel, SnkViewModelRequest request,
                ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
            {
                try
                {
                    viewModel = _viewModelLocator.Reload(viewModel, request.ParameterBundle, savedState,
                        navigationArgs);
                }
                catch (Exception exception)
                {
                    throw new Exception(
                        $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
                }

                return viewModel;
            }
        }
    }
}