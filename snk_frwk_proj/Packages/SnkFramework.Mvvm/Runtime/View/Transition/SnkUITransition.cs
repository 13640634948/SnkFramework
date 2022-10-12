namespace SampleDevelop.Test
{
    public abstract class SnkUITransition : SnkTransitionBase, ISnkUITransition
    {
        protected SnkUITransition(ISnkWindowControllable window) : base(window)
        {
        }
    }
}