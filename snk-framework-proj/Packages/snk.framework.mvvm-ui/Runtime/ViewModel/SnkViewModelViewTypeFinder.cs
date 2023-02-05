using System;
using System.Linq;
using System.Reflection;

using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewModelViewTypeFinder : ISnkViewModelTypeFinder
        {
            private readonly ISnkViewModelByNameLookup _viewModelByNameLookup;
            private readonly ISnkNameMapping _viewToViewModelNameMapping;

            public SnkViewModelViewTypeFinder(ISnkViewModelByNameLookup viewModelByNameLookup,
                ISnkNameMapping viewToViewModelNameMapping)
            {
                _viewModelByNameLookup = viewModelByNameLookup;
                _viewToViewModelNameMapping = viewToViewModelNameMapping;
            }

            public virtual Type? FindTypeOrNull(Type candidateType)
            {
                if (!CheckCandidateTypeIsAView(candidateType))
                    return null;

                if (!candidateType.IsConventional())
                    return null;

                var typeByAttribute = LookupAttributedViewModelType(candidateType);
                if (typeByAttribute != null)
                    return typeByAttribute;

                var concrete = LookupAssociatedConcreteViewModelType(candidateType);
                if (concrete != null)
                    return concrete;

                var typeByName = LookupNamedViewModelType(candidateType);
                if (typeByName != null)
                    return typeByName;

                //MvxLogHost.Default?.Log(LogLevel.Warning, "No view model association found for candidate view {name}", candidateType.Name);
                return null;
            }

            protected virtual Type? LookupAttributedViewModelType(Type candidateType)
            {
                var attribute = candidateType
                    .GetCustomAttributes(typeof(SnkViewForAttribute), false)
                    .FirstOrDefault() as SnkViewForAttribute;

                return attribute?.ViewModel;
            }

            protected virtual Type LookupNamedViewModelType(Type candidateType)
            {
                var viewName = candidateType.Name;
                var viewModelName = _viewToViewModelNameMapping.Map(viewName);

                _viewModelByNameLookup.TryLookupByName(viewModelName, out Type toReturn);
                return toReturn;
            }

            protected virtual Type? LookupAssociatedConcreteViewModelType(Type candidateType)
            {
                var viewModelPropertyInfo = candidateType
                    .GetProperties()
                    .FirstOrDefault(
                        x => x.Name == "ViewModel" &&
                             !x.PropertyType.GetTypeInfo().IsInterface &&
                             !x.PropertyType.GetTypeInfo().IsAbstract);

                return viewModelPropertyInfo?.PropertyType;
            }

            protected virtual bool CheckCandidateTypeIsAView(Type candidateType)
            {
                if (candidateType == null)
                    return false;

                if (candidateType.GetTypeInfo().IsAbstract)
                    return false;

                if (!typeof(ISnkView).IsAssignableFrom(candidateType))
                    return false;

                return true;
            }
        }
    }
}