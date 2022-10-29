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
            public virtual Task<bool> Show(SnkViewModelRequest request)
            {
                var attributeAction = GetPresentationAttributeAction(request, out SnkBasePresentationAttribute attribute);

                if (attributeAction.ShowAction != null && attribute.ViewType != null)
                    return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);

                return Task.FromResult(false);
            }

            public virtual Task<bool> Close(ISnkViewModel viewModel)
            {
                return GetPresentationAttributeAction(
                        new SnkViewModelInstanceRequest(viewModel), out SnkBasePresentationAttribute attribute)
                    .CloseAction?
                    .Invoke(viewModel, attribute) ?? Task.FromResult(false);
            }
        }
    }
}