using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkAttributeViewPresenter
        {
            IDictionary<Type, SnkPresentationAttributeAction> AttributeTypesToActionsDictionary { get; }
            void RegisterAttributeTypes();

            //TODO: Maybe move those to helper class
            SnkBasePresentationAttribute GetPresentationAttribute(SnkViewModelRequest request);
            SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
            SnkBasePresentationAttribute GetOverridePresentationAttribute(SnkViewModelRequest request, Type viewType);
        }
    }
}