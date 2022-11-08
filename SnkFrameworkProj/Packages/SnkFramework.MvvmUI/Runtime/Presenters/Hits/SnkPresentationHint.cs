using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters.Hits
    {
        public abstract class SnkPresentationHint
        {
            protected SnkPresentationHint(SnkBundle body = default)
            {
                Body = body;
            }

            protected SnkPresentationHint(IDictionary<string, string> hints)
                : this(new SnkBundle(hints))
            {
            }

            public SnkBundle Body { get; }
        }
    }
}