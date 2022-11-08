namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        public interface ISnkContainer<T>
        {
            public void Add(T target);
            public bool Remove(T target);
        }
    }
}