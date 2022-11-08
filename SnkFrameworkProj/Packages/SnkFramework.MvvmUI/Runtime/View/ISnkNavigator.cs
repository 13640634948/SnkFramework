namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkNavigator
    {
        public ISnkView Current { get; }
        public ISnkView NavigatorPrev { get; }
        public ISnkView NavigatorNext { get; }
    }
}