using System;
using System.Collections.Generic;
using System.Linq;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.Presenters.Hits;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public partial class SnkViewPresenter
        {
            public override void RegisterAttributeTypes()
            {
                AttributeTypesToActionsDictionary.Register<SnkPresentationWindowAttribute>(OpenWindow, CloseWindow);
                //internalRegisterAttributeTypes<>();
            }

            public override SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
            {
                var attribute = new SnkPresentationWindowAttribute();
                attribute.ViewModelType = viewModelType;
                attribute.ViewType = viewType;
                return attribute;
            }

            public override SnkBasePresentationAttribute GetOverridePresentationAttribute(SnkViewModelRequest request,
                Type viewType)
                => null;

            protected virtual SnkPresentationAttributeAction GetPresentationAttributeAction(SnkViewModelRequest request,
                out SnkBasePresentationAttribute attribute)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var presentationAttribute = GetPresentationAttribute(request);
                presentationAttribute.ViewModelType = request.ViewModelType;
                var attributeType = presentationAttribute.GetType();

                attribute = presentationAttribute;

                if (AttributeTypesToActionsDictionary != null &&
                    AttributeTypesToActionsDictionary.TryGetValue(attributeType,
                        out SnkPresentationAttributeAction attributeAction))
                {
                    if (attributeAction.OpenAction == null)
                    {
                        throw new InvalidOperationException(
                            $"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
                    }

                    if (attributeAction.CloseAction == null)
                    {
                        throw new InvalidOperationException(
                            $"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
                    }

                    return attributeAction;
                }

                throw new KeyNotFoundException(
                    $"The type {attributeType.Name} is not configured in the presenter dictionary");
            }

            public override SnkBasePresentationAttribute GetPresentationAttribute(SnkViewModelRequest request)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (request.ViewModelType == null)
                    throw new InvalidOperationException("Cannot get view types for null ViewModelType");

                //if (ViewsContainer == null)
                //    throw new InvalidOperationException($"Cannot get view types from null {nameof(ViewsContainer)}");

                //var viewType = ViewsContainer.GetViewType(request.ViewModelType);
                Type viewType = this._viewFinder.GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new InvalidOperationException(
                        $"Could not get View Type for ViewModel Type {request.ViewModelType}");

                var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
                if (overrideAttribute != null)
                    return overrideAttribute;

                var attribute = viewType
                    .GetCustomAttributes(typeof(SnkBasePresentationAttribute), true)
                    .FirstOrDefault();

                if (attribute is SnkBasePresentationAttribute basePresentationAttribute)
                {
                    if (basePresentationAttribute.ViewType == null)
                        basePresentationAttribute.ViewType = viewType;

                    if (basePresentationAttribute.ViewModelType == null)
                        basePresentationAttribute.ViewModelType = request.ViewModelType;

                    return basePresentationAttribute;
                }

                return CreatePresentationAttribute(request.ViewModelType, viewType);
            }
            
        }
    }
}