namespace SampleDevelop.Test
{
    public abstract class SnkUITransition : SnkTransitionBase, ISnkUITransition
    {
        protected SnkUITransition(ISnkControllable window) : base(window)
        {
        }
    }
}