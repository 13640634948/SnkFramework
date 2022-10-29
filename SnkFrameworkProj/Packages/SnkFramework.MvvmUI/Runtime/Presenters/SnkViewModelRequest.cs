using System;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public class SnkViewModelRequest
    {
        public Type ViewModelType { get; set; }
        public Type ViewType { get; set; }
    }

    public class SnkViewModelInstanceRequest : SnkViewModelRequest
    {
        public ISnkViewModel ViewModelInstance { get; set; }

        public SnkViewModelInstanceRequest()
        {
            
        }

        public SnkViewModelInstanceRequest(ISnkViewModel viewModelInstance)
        {
            ViewModelInstance = viewModelInstance;
        }
    }
}