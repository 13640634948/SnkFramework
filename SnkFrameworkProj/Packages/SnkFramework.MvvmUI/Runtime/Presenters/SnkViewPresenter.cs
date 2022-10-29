using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public partial class SnkViewPresenter : SnkViewAttributeOrganizer, ISnkViewPresenter
        {
            public virtual Task<bool> Open(SnkViewModelRequest request)
            {
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);

                if (attributeAction.OpenAction != null && attribute.ViewType != null)
                    return attributeAction.OpenAction.Invoke(attribute, request);

                return Task.FromResult(false);
            }

            public virtual Task<bool> Close(ISnkViewModel viewModel)
            {
                SnkViewModelRequest request = new SnkViewModelRequest();
                request.ViewModelInstance = viewModel;
                var attributeAction = GetPresentationAttributeAction(request, out var attribute);
                if (attributeAction.CloseAction != null)
                    return attributeAction.CloseAction.Invoke(viewModel, attribute);
                return Task.FromResult(false);
            }
        }
    }
}