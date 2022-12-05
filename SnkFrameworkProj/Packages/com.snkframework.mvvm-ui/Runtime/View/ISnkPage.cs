using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkPage : ISnkView, ISnkNavigator, ISnkContainer<ISnkView>
    {
        public ISnkWindow ParentWindow { get; }
    }
}