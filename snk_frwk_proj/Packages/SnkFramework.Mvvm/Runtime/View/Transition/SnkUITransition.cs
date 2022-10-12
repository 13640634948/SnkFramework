namespace SnkFramework.Mvvm.View
{
    public abstract class SnkUITransition : SnkTransition, ISnkUITransition
    {
        protected SnkUITransition(ISnkWindowControllable window) : base(window)
        {
        }
    }
}