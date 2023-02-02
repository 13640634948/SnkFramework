using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewModelLoader : SnkViewModelLocator, ISnkViewModelLoader
        {
            public ISnkViewModel LoadViewModel(SnkViewModelRequest request, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null)
            {
                try
                {
                    return this.Load(request.ViewModelType!, request.ParameterBundle, savedState, navigationArgs);
                }
                catch (Exception exception)
                {
                    throw new Exception(
                        $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {GetType().Name} - check InnerException for more information");
                }
            }

            public ISnkViewModel ReloadViewModel(ISnkViewModel viewModel, SnkViewModelRequest request,
                ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null)
            {
                try
                {
                    viewModel = this.Reload(viewModel, request.ParameterBundle, savedState,
                        navigationArgs);
                }
                catch (Exception exception)
                {
                    throw new Exception(
                        $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {GetType().Name} - check InnerException for more information");
                }

                return viewModel;
            }
        }
    }
}