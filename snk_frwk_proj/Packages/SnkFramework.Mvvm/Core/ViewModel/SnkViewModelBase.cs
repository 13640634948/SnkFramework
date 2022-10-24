using Loxodon.Framework.Observables;
using SnkFramework.Mvvm.Core.Log;

namespace SnkFramework.Mvvm.Core
{
    namespace ViewModel
    {
        public abstract class SnkViewModelBase : ObservableObject, ISnkViewModel
        {
            protected static readonly ISnkMvvmLogger Logger = SnkIoCProvider.Instance.Resolve<ISnkMvvmLogger>();
        }
    }
}