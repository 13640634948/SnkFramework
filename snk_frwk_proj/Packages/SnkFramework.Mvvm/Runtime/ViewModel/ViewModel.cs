
using Loxodon.Framework.Observables;

namespace SnkFramework.Mvvm.ViewModel
{
    public interface IViewModel
    {
    }

    public abstract class ViewModel : ObservableObject,IViewModel
    {
    }
}