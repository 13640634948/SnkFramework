using System;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.ViewModel;

public class SnkViewFinder : ISnkViewFinder
{
    public Type GetViewType(Type viewModelType)
    {
        return typeof(TestWindow);
    }

    public Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel
        => GetViewType(typeof(TViewModel));
}