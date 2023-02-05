using System;

namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public interface IMvxTypeFinder
    {
        Type? FindTypeOrNull(Type candidateType);
    }
}