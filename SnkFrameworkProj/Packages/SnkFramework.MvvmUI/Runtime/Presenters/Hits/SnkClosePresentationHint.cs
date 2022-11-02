using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters.Hits
    {
        public class SnkClosePresentationHint : SnkPresentationHint
        {
            public SnkClosePresentationHint(ISnkViewModel viewModelToClose)
            {
                ViewModelToClose = viewModelToClose;
            }

            public SnkClosePresentationHint(ISnkViewModel viewModelToClose, SnkBundle body) : base(body)
            {
                ViewModelToClose = viewModelToClose;
            }

            public SnkClosePresentationHint(ISnkViewModel viewModelToClose, IDictionary<string, string> hints) : this(
                viewModelToClose, new SnkBundle(hints))
            {
            }

            public ISnkViewModel ViewModelToClose { get; }
        }
    }
}