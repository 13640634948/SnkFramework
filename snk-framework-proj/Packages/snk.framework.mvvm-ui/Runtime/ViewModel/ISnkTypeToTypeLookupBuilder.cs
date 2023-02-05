using System;
using System.Collections.Generic;
using System.Reflection;

namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public interface ISnkTypeToTypeLookupBuilder
    {
        IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies);
    }
}