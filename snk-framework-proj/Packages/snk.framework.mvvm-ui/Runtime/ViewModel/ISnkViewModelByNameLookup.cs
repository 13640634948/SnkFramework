using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModelByNameLookup
        {
            bool TryLookupByName(string name, out Type viewModelType);

            bool TryLookupByFullName(string name, out Type viewModelType);
        }
    }
}