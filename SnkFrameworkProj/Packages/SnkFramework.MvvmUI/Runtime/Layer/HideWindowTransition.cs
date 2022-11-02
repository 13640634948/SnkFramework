using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public class HideWindowTransition : SnkTransition
    {
        private bool _dismiss;

        public HideWindowTransition(ISnkWindow window, bool dismiss) : base(window)
        {
            this._dismiss = dismiss;
        }
    }
}