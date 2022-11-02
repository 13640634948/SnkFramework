using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public class ShowWindowTransition : SnkTransition
    {
        public ShowWindowTransition(ISnkWindow window) : base(window)
        {
        }
    }
}