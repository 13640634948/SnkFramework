using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public class SnkViewModelRequest
    {
        public Type ViewModelType { get; set; }

        public ISnkBundle ParameterBundle { get; set; }

        public ISnkBundle PresentationBundle { get; set; }

        public SnkViewModelRequest(Type viewModelType,
            ISnkBundle parameterBundle = null,
            ISnkBundle presentationBundle = null)
        {
            ViewModelType = viewModelType;
            ParameterBundle = parameterBundle;
            PresentationBundle = presentationBundle;
        }
    } 
    
    public class SnkViewModelRequest<TViewModel> : SnkViewModelRequest
        where TViewModel : class, ISnkViewModel
    {
        public SnkViewModelRequest() : base(typeof(TViewModel))
        {
        }

        public SnkViewModelRequest(ISnkBundle parameterBundle = null, ISnkBundle presentationBundle = null)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }
    }
}