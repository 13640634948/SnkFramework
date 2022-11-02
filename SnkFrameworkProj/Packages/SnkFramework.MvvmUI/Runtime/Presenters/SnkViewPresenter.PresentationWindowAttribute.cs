using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public partial class SnkViewPresenter
    {
        protected virtual async Task<bool> OpenWindow(ISnkPresentationAttribute attribute, SnkViewModelRequest request)
        {
            SnkWindow window = await this._viewLoader.CreateView(request);
            SnkUGUINormalLayer layer = _layerContainer.GetLayer<SnkUGUINormalLayer>();
            layer.AddChild(window);
            return await layer.Open(window);
        }

        protected virtual async Task<bool> CloseWindow(ISnkViewModel viewModel, ISnkPresentationAttribute attribute)
        {
            Debug.LogError("SnkViewPresenter.CloseWindow");
            SnkUGUINormalLayer layer = _layerContainer.GetLayer<SnkUGUINormalLayer>();
            SnkWindow window = layer.GetChild(0);
            Debug.LogError("SnkViewPresenter.window:" + window);
            var result = await layer.Close(window);
            if (result == false)
                return false;
            Debug.LogError("SnkViewPresenter.UnloadView:" + window);
            this._viewLoader.UnloadView(window);
            return true;
        }
    }
}