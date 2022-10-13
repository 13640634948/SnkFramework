namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkViewControllable
        {
            public ISnkAnimation mEnterAnimation { get; set; }
            public ISnkAnimation mExitAnimation { get; set; }
        }
    }
}