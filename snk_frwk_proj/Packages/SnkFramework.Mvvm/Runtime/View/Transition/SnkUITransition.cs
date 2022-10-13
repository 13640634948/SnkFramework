namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public abstract class SnkUITransition : SnkTransition, ISnkUITransition
        {
            protected SnkUITransition(ISnkWindowControllable window) : base(window)
            {
            }
        }
    }
}