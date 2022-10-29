using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.Presenters
{
    public class SnkViewModelRequest
    {
        public Type ViewModelType { get; set; }
        public ISnkViewModel ViewModelInstance { get; set; }
        public object Parameter { get; set; }
        public Type ViewType { get; set; }
        public ISnkView ViewInstance { get; set; }
        public ISnkBundle Bundle { get; set; }
    }
}