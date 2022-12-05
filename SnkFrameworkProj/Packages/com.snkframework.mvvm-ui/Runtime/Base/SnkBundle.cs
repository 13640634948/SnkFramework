using System.Collections.Generic;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        public class SnkBundle : ISnkBundle
        {
            public SnkBundle() : this(new Dictionary<string, string>())
            {
            }

            public SnkBundle(IDictionary<string, string>? data)
            {
            }
        }
    }
}