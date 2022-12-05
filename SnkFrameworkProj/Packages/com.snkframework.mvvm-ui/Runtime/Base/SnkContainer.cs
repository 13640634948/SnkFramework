using System.Collections.Generic;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        public class SnkContainer<T> : ISnkContainer<T>
        {
            private List<T> _targetList = new List<T>();

            public void Add(T target)
            {
                _targetList.Add(target);
            }

            public bool Remove(T target)
            {
                return _targetList.Remove(target);
            }
        }
    }
}