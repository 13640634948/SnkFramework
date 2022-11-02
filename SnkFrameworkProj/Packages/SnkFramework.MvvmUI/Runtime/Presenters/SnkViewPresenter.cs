using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public abstract partial class SnkViewPresenter : SnkViewAttributeOrganizer, ISnkViewPresenter
        {
            private ISnkViewFinder _viewFinder;
            private ISnkViewLoader _viewLoader;
            private ISnkLayerContainer _layerContainer;
            private readonly Dictionary<Type, Func<SnkPresentationHint, Task<bool>>> _presentationHintHandlers =
                new Dictionary<Type, Func<SnkPresentationHint, Task<bool>>>();

            public SnkViewPresenter(ISnkViewFinder viewFinder, ISnkViewLoader viewLoader, ISnkLayerContainer layerContainer)
            {
                this._viewFinder = viewFinder;
                this._viewLoader = viewLoader;
                this._layerContainer = layerContainer;
            }

            public virtual Task<bool> Open(SnkViewModelRequest request)
            {
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);

                Debug.Log(attributeAction.OpenAction);
                Debug.Log(attribute.ViewType);
                if (attributeAction.OpenAction != null && attribute.ViewType != null)
                    return attributeAction.OpenAction.Invoke(attribute, request);

                return Task.FromResult(false);
            }
            public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
                where THint : SnkPresentationHint
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
            }
            protected Task<bool> HandlePresentationChange(SnkPresentationHint hint)
            {
                if (hint == null)
                    throw new ArgumentNullException(nameof(hint));

                if (_presentationHintHandlers.TryGetValue(
                        hint.GetType(),
                        out Func<SnkPresentationHint, Task<bool>> handler))
                {
                    return handler(hint);
                }

                return Task.FromResult(false);
            }


            public async Task<bool> ChangePresentation(SnkPresentationHint hint)
            {
                if (await HandlePresentationChange(hint).ConfigureAwait(true))
                {
                    Debug.LogError("ChangePresentation-HandlePresentationChange");
                    return true;                    
                }


                if (hint is SnkClosePresentationHint presentationHint)
                {
                    Debug.LogError("ChangePresentation-hint:" + hint);
                    return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
                }

                //MvxLogHost.Default?.Log(LogLevel.Warning, "Hint ignored {name}", hint.GetType().Name);
                return false;
            }

            public virtual Task<bool> Close(ISnkViewModel viewModel)
            {
                var request = new SnkViewModelInstanceRequest(viewModel);
                request.ViewModelInstance = viewModel;
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);
                if (attributeAction.CloseAction != null)
                    return attributeAction.CloseAction.Invoke(viewModel, attribute);
                return Task.FromResult(false);
            }
        }
    }
}