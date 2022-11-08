using System;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkViewFinder : ISnkViewFinder
        {
            public abstract Type GetViewType(Type viewModelType);

            public virtual Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel
                => GetViewType(typeof(TViewModel));
        }
    }
}