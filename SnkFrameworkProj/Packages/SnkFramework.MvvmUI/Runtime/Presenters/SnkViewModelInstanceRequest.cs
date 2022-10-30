using System;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public class SnkViewModelInstanceRequest : SnkViewModelRequest
    {
        public ISnkViewModel ViewModelInstance { get; set; }

        public SnkViewModelInstanceRequest(Type viewModelType)
            : base(viewModelType)
        {
        }

        public SnkViewModelInstanceRequest(ISnkViewModel viewModelInstance)
            : base(viewModelInstance.GetType(), null, null)
        {
            ViewModelInstance = viewModelInstance;
        }
    }
}