using System;

namespace SnkFramework.Runtime.Core
{
    public interface ISnkViewModelByNameLookup
    {
        
        bool TryLookupByName(string name, out Type viewModelType);

        bool TryLookupByFullName(string name, out Type viewModelType);
    }
}