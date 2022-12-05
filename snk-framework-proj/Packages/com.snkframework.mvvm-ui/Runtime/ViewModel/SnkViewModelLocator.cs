using System;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewModelLocator : ISnkViewModelLocator
        {
            private ISnkViewModelCreator _viewModelCreator;

            public SnkViewModelLocator(ISnkViewModelCreator viewModelCreator)
            {
                _viewModelCreator = viewModelCreator;
            }

            public ISnkViewModel Load(Type viewModelType, ISnkBundle parameterValues, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null)
            {
                if (viewModelType == null)
                    throw new ArgumentNullException(nameof(viewModelType));

                ISnkViewModel viewModel;
                try
                {
                    viewModel = this._viewModelCreator?.CreateViewModel(viewModelType);
                }
                catch (Exception exception)
                {
                    throw new Exception($"Problem creating viewModel of type {viewModelType.Name}");
                }
                RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);
                return viewModel;
            }


            public ISnkViewModel Reload(ISnkViewModel viewModel, ISnkBundle parameterValues, ISnkBundle savedState,
                ISnkNavigateEventArgs navigationArgs = null)
            {
                RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);
                return viewModel;
            }


            protected void RunViewModelLifecycle(ISnkViewModel viewModel, ISnkBundle parameterValues,
                ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs)
            {
                if (viewModel == null)
                    throw new ArgumentNullException(nameof(viewModel));

                try
                {
                    viewModel.Init(parameterValues);

                    if (navigationArgs?.Cancel == true)
                        return;
                    
                    if (savedState != null)
                    {
                        viewModel.ReloadState(savedState);

                        if (navigationArgs?.Cancel == true)
                            return;
                    }

                    viewModel.Start();
                    if (navigationArgs?.Cancel == true)
                        return;

                    viewModel.Prepare(null);
                    if (navigationArgs?.Cancel == true)
                        return;

                    viewModel.InitializeTask = SnkNotifyTask.Create(() => viewModel.Initialize());
                }
                catch (Exception exception)
                {
                    throw new Exception($"Problem running viewModel lifecycle of type {viewModel.GetType().Name}");
                }
            }
        }
    }
}