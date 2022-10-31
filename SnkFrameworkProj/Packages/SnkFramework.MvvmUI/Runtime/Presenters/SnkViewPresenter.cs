using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
 
        public interface ISnkViewLoader
        {
            Task<SnkUIBehaviour> CreateView(SnkViewModelRequest request);

            Task<SnkUIBehaviour> CreateView(Type viewType);
        }
        
        public partial class SnkViewPresenter : SnkViewAttributeOrganizer, ISnkViewPresenter
        {
            private ISnkViewFinder _viewFinder;
            private ISnkViewLoader _viewLoader;

            public SnkViewPresenter(ISnkViewFinder viewFinder, ISnkViewLoader viewLoader)
            {
                this._viewFinder = viewFinder;
                this._viewLoader = viewLoader;
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