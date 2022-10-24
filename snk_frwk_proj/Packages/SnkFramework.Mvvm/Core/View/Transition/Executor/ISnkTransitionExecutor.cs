namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkTransitionExecutor
        {
            public void Execute(SnkTransition transition);
        }
    }
}