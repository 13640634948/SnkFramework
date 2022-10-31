using System;
using SnkFramework.Mvvm.Runtime.ViewModel;

public class SnkViewModelCreator : ISnkViewModelCreator
{
    public ISnkViewModel CreateViewModel(Type viewModelType)
        => Activator.CreateInstance(viewModelType) as ISnkViewModel;
}