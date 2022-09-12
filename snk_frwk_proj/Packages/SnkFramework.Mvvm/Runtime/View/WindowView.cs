using SnkFramework.Mvvm.Base;

namespace SnkFramework.Mvvm.View
{
    public abstract class WindowView : View, IWindowView
    {
        public virtual UIAnimation ActivationAnimation { get; }
        public virtual UIAnimation PassivationAnimation { get; }
    }
}