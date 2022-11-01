using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
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
        public class SnkClosePresentationHint
            : SnkPresentationHint
        {
            public SnkClosePresentationHint(ISnkViewModel viewModelToClose)
            {
                ViewModelToClose = viewModelToClose;
            }

            public SnkClosePresentationHint(ISnkViewModel viewModelToClose, SnkBundle body) : base(body)
            {
                ViewModelToClose = viewModelToClose;
            }

            public SnkClosePresentationHint(ISnkViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new SnkBundle(hints))
            {
            }

            public ISnkViewModel ViewModelToClose { get; }
        }
        
        public interface ISnkViewPresenter
        {
            Task<bool> Open(SnkViewModelRequest request);

            Task<bool> ChangePresentation(SnkPresentationHint hint);

            void AddPresentationHintHandler<THint>(System.Func<THint, Task<bool>> action) where THint : SnkPresentationHint;

            Task<bool> Close(ISnkViewModel viewModel);
        }
    }
}