using System;
using SnkFramework.Mvvm.Demo.Implements.Layers;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Demo
{
    namespace Implements
    {
        public class DemoPresenter : SnkViewPresenter
        {
            public override SnkBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
            {
                var attribute = base.CreatePresentationAttribute(viewModelType, viewType);
                if (attribute is SnkPresentationWindowAttribute windowAttribute)
                {
                    windowAttribute.LayerType = typeof(DemoUGUINormalLayer);
                }

                return attribute;
            }
        }
    }
}