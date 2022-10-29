using System;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public partial class SnkViewPresenter
    {
        
        public override void RegisterAttributeTypes()
        {
        }

        public override SnkBasePresentationAttribute GetPresentationAttribute(SnkViewModelRequest request)
        {
            return default;
        }

        public override SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            return default;
        }

        public override SnkBasePresentationAttribute GetOverridePresentationAttribute(SnkViewModelRequest request, Type viewType)
        {
            return default;
        }

        protected virtual SnkPresentationAttributeAction GetPresentationAttributeAction(SnkViewModelRequest request, out SnkBasePresentationAttribute attribute)
        {
            attribute = default;
            return default;
        }
    }
}