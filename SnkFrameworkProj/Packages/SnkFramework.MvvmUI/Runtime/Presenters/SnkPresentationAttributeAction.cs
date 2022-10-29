using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public class SnkPresentationAttributeAction
        {
            public Func<ISnkPresentationAttribute, SnkViewModelRequest, Task<bool>> OpenAction { get; set; }

            public Func<ISnkViewModel, ISnkPresentationAttribute, Task<bool>> CloseAction { get; set; }
        }
    }
}