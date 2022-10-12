namespace SampleDevelop.Test
{
    public interface ISnkWindowView : ISnkUIPage
    {
        public ISnkAnimation mActivationAnimation { get; set; }
        public ISnkAnimation mPassivationAnimation { get; set; }
    }

    public interface ISnkWindowView<TViewOwner> : ISnkWindowView, ISnkUIPage<TViewOwner>
        where TViewOwner : class, ISnkViewOwner
    {
        
    }
}