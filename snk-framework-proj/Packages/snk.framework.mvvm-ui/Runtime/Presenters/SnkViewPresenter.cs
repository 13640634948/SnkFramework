using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public partial class SnkViewPresenter : SnkViewAttributeOrganizer, ISnkViewPresenter
        {
            private Lazy<ISnkViewsContainer> _viewsContainer = new (()=> 
                SnkSingleton<ISnkIoCProvider>.Instance.Resolve<ISnkViewsContainer>());

            //private Lazy<ISnkViewCreator> _viewLoader = new(() =>
            //    SnkSingleton<ISnkIoCProvider>.Instance.Resolve<ISnkViewCreator>());
            
            private Lazy<ISnkLayerContainer> _layerContainer = new(() =>
                SnkSingleton<ISnkIoCProvider>.Instance.Resolve<ISnkLayerContainer>());
            
            public ISnkViewsContainer viewsContainer => this._viewsContainer.Value;
            //public ISnkViewCreator ViewCreator => this._viewLoader.Value;
            public ISnkLayerContainer LayerContainer => this._layerContainer.Value;
            

            private readonly Dictionary<Type, Func<SnkPresentationHint, Task<bool>>> _presentationHintHandlers =
                    new Dictionary<Type, Func<SnkPresentationHint, Task<bool>>>();
            public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
                where THint : SnkPresentationHint
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
            }

            protected async Task<bool> HandlePresentationChange(SnkPresentationHint hint)
            {
                if (hint == null)
                    throw new ArgumentNullException(nameof(hint));

                if (_presentationHintHandlers.TryGetValue(hint.GetType(), out var handler))
                {
                    return await handler(hint);
                }

                return false;
            }


            public async Task<bool> ChangePresentation(SnkPresentationHint hint)
            {
                /*
                if (await HandlePresentationChange(hint))
                {
                    Debug.LogError("ChangePresentation-HandlePresentationChange");
                    return true;
                }

                if (hint is SnkClosePresentationHint presentationHint)
                {
                    Debug.LogError("ChangePresentation-hint:" + hint);
                    return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
                }
                //SnkLogHost.Default?.Log(LogLevel.Warning, "Hint ignored {name}", hint.GetType().Name);
                return false;
                */
                return false;
            }


            public virtual async Task<bool> Open(SnkViewModelRequest request)
            {
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);
                if (attributeAction.OpenAction != null && attribute.ViewType != null)
                    return await attributeAction.OpenAction.Invoke(attribute, request);
                return false;
            }

            public virtual async Task<bool> Close(ISnkViewModel viewModel)
            {
                var request = new SnkViewModelInstanceRequest(viewModel);
                request.ViewModelInstance = viewModel;
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);
                if (attributeAction.CloseAction != null)
                    return await attributeAction.CloseAction.Invoke(viewModel, attribute);
                return false;
            }
        }
    }
}