namespace SnkFramework.Mvvm.View
{
    public abstract class SnkUITransition : SnkTransitionBase, ISnkUITransition
    {
        protected SnkUITransition(ISnkWindowControllable window) : base(window)
        {
        }
    }
}