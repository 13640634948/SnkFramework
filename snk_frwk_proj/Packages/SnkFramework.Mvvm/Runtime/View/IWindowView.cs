
using SnkFramework.Mvvm.Base;

namespace SnkFramework.Mvvm.View
{
    public interface IWindowView : IView
    {
        public UIAnimation ActivationAnimation { get; }
        public UIAnimation PassivationAnimation { get; }
        
    }
}