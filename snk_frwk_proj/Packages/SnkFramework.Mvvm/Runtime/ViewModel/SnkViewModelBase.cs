using Loxodon.Framework.Observables;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.Log;

namespace SnkFramework.Mvvm.ViewModel
{
    public abstract class SnkViewModelBase : ObservableObject,ISnkViewModel
    {
        protected static readonly IMvvmLog log = SnkMvvmSetup.mMvvmLog;
    }
}