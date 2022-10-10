namespace SampleDevelop.Test
{
    public interface ISnkTransitionController
    {
        public ISnkTransition Show(ISnkWindow window);
        public ISnkTransition Hide(ISnkWindow window);
        public ISnkTransition Dismiss(ISnkWindow window);
    }
}