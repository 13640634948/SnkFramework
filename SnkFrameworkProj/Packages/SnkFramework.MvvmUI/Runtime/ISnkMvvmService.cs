using System.Threading;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SnkFramework.Mvvm.Runtime
{
    public interface ISnkMvvmService
    {
        public Task<TViewModel> OpenWindow<TViewModel>(ISnkBundle presentationBundle = null)
            where TViewModel : class, ISnkViewModel;

        public Task<bool> CloseWindow(ISnkViewModel viewModel);
    }
}