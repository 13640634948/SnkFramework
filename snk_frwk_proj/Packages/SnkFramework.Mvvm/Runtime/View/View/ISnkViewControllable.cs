namespace SnkFramework.Mvvm.View
{
    public interface ISnkViewControllable
    {
        public ISnkAnimation mEnterAnimation { get; set; }
        public ISnkAnimation mExitAnimation { get; set; }
    }
}