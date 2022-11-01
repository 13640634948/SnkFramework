using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public partial class SnkViewPresenter
    {
        protected virtual async Task<bool> ShowWindow(ISnkPresentationAttribute attribute, SnkViewModelRequest request)
        {
            SnkUIBehaviour viewBehaviour = await this._viewLoader.CreateView(request);
            SnkUGUINormalLayer layer = _layerContainer.GetLayer<SnkUGUINormalLayer>();
            layer.AddChild(viewBehaviour);
            return await layer.Show((ISnkWindow)viewBehaviour);
        }

        protected virtual async Task<bool> CloseWindow(ISnkViewModel viewModel, ISnkPresentationAttribute attribute)
        {
            return true;
        }
    }
}