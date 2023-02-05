using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkTypeFinder
        {
            Type? FindTypeOrNull(Type candidateType);
        }
    }
}