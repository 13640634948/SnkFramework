using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public abstract class SnkAttributeViewPresenter : SnkViewPresenter, ISnkAttributeViewPresenter
        {
            private IDictionary<Type, SnkPresentationAttributeAction> _attributeTypesToActionsDictionary;

            public virtual IDictionary<Type, SnkPresentationAttributeAction> AttributeTypesToActionsDictionary
            {
                get
                {
                    if (_attributeTypesToActionsDictionary == null)
                    {
                        _attributeTypesToActionsDictionary = new Dictionary<Type, SnkPresentationAttributeAction>();
                        RegisterAttributeTypes();
                    }

                    return _attributeTypesToActionsDictionary;
                }
            }

            public abstract void RegisterAttributeTypes();

            public abstract SnkBasePresentationAttribute GetPresentationAttribute(SnkViewModelRequest request);

            public abstract SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

            public abstract SnkBasePresentationAttribute? GetOverridePresentationAttribute(SnkViewModelRequest request, Type viewType);

        }
    }
}