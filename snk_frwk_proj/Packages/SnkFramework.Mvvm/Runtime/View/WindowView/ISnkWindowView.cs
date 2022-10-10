namespace SampleDevelop.Test
{
    public interface ISnkWindowView : ISnkUIPage
    {
        public ISnkAnimation mActivationAnimation { get; set; }
        public ISnkAnimation mPassivationAnimation { get; set; }
    }
}