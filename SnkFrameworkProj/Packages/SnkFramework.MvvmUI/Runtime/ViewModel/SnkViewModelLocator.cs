using System;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime.ViewModel
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
                //viewModel = (IMvxViewModel)Mvx.IoCProvider.IoCConstruct(viewModelType);
                viewModel = this._viewModelCreator?.CreateViewModel(viewModelType);

            }
            catch (Exception exception)
            {
                throw new Exception($"Problem creating viewModel of type {viewModelType.Name}");
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public ISnkViewModel<TParameter> Load<TParameter>(Type viewModelType, TParameter param, ISnkBundle parameterValues, ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null) where TParameter : notnull
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            ISnkViewModel<TParameter> viewModel;
            try
            {
                //viewModel = (IMvxViewModel<TParameter>)Mvx.IoCProvider.IoCConstruct(viewModelType);
                viewModel = this._viewModelCreator?.CreateViewModel(viewModelType) as  ISnkViewModel<TParameter>;
            }
            catch (Exception exception)
            {
                throw new Exception($"Problem creating viewModel of type {viewModelType.Name}");
            }

            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public ISnkViewModel Reload(ISnkViewModel viewModel, ISnkBundle parameterValues, ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs = null)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);
            return viewModel;
        }

        public ISnkViewModel<TParameter> Reload<TParameter>(ISnkViewModel<TParameter> viewModel, TParameter param, ISnkBundle parameterValues,
            ISnkBundle savedState, ISnkNavigateEventArgs navigationArgs = null) where TParameter : notnull
        {
            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);
            return viewModel;
        }
        
        protected void RunViewModelLifecycle(
            ISnkViewModel viewModel,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                //CallCustomInitMethods(viewModel, parameterValues);
                viewModel.Init(parameterValues);

                if (navigationArgs?.Cancel == true)
                    return;
                if (savedState != null)
                {
                    //CallReloadStateMethods(viewModel, savedState);
                    viewModel.ReloadState(savedState);

                    if (navigationArgs?.Cancel == true)
                        return;
                }
                viewModel.Start();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.InitializeTask = SnkNotifyTask.Create(() => viewModel.Initialize());
            }
            catch (Exception exception)
            {
                throw new Exception($"Problem running viewModel lifecycle of type {viewModel.GetType().Name}");
            }
        }

        protected void RunViewModelLifecycle<TParameter>(
            ISnkViewModel<TParameter> viewModel,
            TParameter param,
            ISnkBundle parameterValues,
            ISnkBundle savedState,
            ISnkNavigateEventArgs navigationArgs)
            where TParameter : notnull
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            try
            {
                //CallCustomInitMethods(viewModel, parameterValues);
                viewModel.Init(parameterValues);
                if (navigationArgs?.Cancel == true)
                    return;
                if (savedState != null)
                {
                    //CallReloadStateMethods(viewModel, savedState);
                    viewModel.ReloadState(savedState);
                    if (navigationArgs?.Cancel == true)
                        return;
                }
                viewModel.Start();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare();
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.Prepare(param);
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