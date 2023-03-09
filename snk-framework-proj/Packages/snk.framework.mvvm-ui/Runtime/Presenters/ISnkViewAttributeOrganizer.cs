using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkViewAttributeOrganizer
        {
            IDictionary<Type, SnkPresentationAttributeAction> AttributeTypesToActionsDictionary { get; }
            void RegisterAttributeTypes();

            SnkBasePresentationAttribute GetPresentationAttribute(SnkViewModelRequest request);
            SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
            SnkBasePresentationAttribute GetOverridePresentationAttribute(SnkViewModelRequest request, Type viewType);
        }
    }
}