using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public partial class SnkViewPresenter
        {
            protected virtual async Task<bool> OpenWindow(ISnkPresentationAttribute attribute, SnkViewModelRequest request)
            {
                var window = await this._getViewsContainer.CreateView(request);
                if (attribute is not SnkPresentationWindowAttribute windowAttribute)
                    throw new ArgumentNullException(nameof(windowAttribute) + " is null");
                var layer = _getLayerContainer.GetLayer(windowAttribute.LayerType);
                window.Create(layer, null);
                await layer.Open(window);
                return true;
            }

            protected virtual async Task<bool> CloseWindow(ISnkViewModel viewModel, ISnkPresentationAttribute attribute)
            {
                if (attribute is not SnkPresentationWindowAttribute windowAttribute)
                    throw new ArgumentNullException(nameof(windowAttribute) + " is null");
                var layer = _getLayerContainer.GetLayer(windowAttribute.LayerType);
                var window = layer.GetChild(0);
                await layer.Close(window);
                this._getViewsContainer.UnloadView(window);
                return true;
            }
        }
    }
}