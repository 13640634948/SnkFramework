using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkViewsContainer : ISnkViewFinder, ISnkViewCreator
    {
        void AddAll(IDictionary<Type, Type> viewModelViewLookup);

        void Add(Type viewModelType, Type viewType);

        void Add<TViewModel, TView>()
            where TViewModel : ISnkViewModel
            where TView : ISnkView;

        void AddSecondary(ISnkViewFinder finder);

        void SetLastResort(ISnkViewFinder finder);
        
    }
}