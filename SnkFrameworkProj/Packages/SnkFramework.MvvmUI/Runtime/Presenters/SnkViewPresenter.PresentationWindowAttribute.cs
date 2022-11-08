using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public partial class SnkViewPresenter
        {
            protected virtual async Task<bool> OpenWindow(ISnkPresentationAttribute attribute, SnkViewModelRequest request)
            {
                try
                {

                    SnkWindow window = await this._viewLoader.CreateView(request);
                    var windowAttribute = attribute as SnkPresentationWindowAttribute;
                    var layer = _layerContainer.GetLayer(windowAttribute.LayerType);
                    layer.AddChild(window);
                    Debug.Log("Presenter.OpenWindow-Begin");
                    await layer.Open(window);
                    Debug.Log("Presenter.OpenWindow-End");

                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw;
                }
                return true;
            }

            protected virtual async Task<bool> CloseWindow(ISnkViewModel viewModel, ISnkPresentationAttribute attribute)
            {
                var windowAttribute = attribute as SnkPresentationWindowAttribute;
                var layer = _layerContainer.GetLayer(windowAttribute.LayerType);
                SnkWindow window = layer.GetChild(0);
                await layer.Close(window);
                this._viewLoader.UnloadView(window);
                return true;
            }
        }
    }
}