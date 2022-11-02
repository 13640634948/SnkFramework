using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters.Attributes;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public static class MvxPresentationAttributeExtensions
        {
            public static bool HasBasePresentationAttribute(this Type candidateType)
            {
                var attributes = candidateType.GetCustomAttributes(typeof(SnkBasePresentationAttribute), true);
                return attributes.Length > 0;
            }

            public static IEnumerable<SnkBasePresentationAttribute> GetBasePresentationAttributes(
                this Type fromViewType)
            {
                var attributes = fromViewType.GetCustomAttributes(typeof(SnkBasePresentationAttribute), true);

                if (attributes.Length == 0)
                    throw new InvalidOperationException(
                        $"Type does not have {nameof(SnkBasePresentationAttribute)} attribute!");

                return attributes.Cast<SnkBasePresentationAttribute>();
            }

            public static SnkBasePresentationAttribute GetBasePresentationAttribute(this Type fromViewType)
            {
                return fromViewType.GetBasePresentationAttributes().FirstOrDefault();
            }

            public static Type? GetViewModelType(this Type viewType)
            {
                if (!viewType.HasBasePresentationAttribute())
                    return null;

                return viewType.GetBasePresentationAttributes()
                    .Select(x => x.ViewModelType)
                    .FirstOrDefault();
            }

            public static void Register<TSnkPresentationAttribute>(
                this IDictionary<Type, SnkPresentationAttributeAction> attributeTypesToActionsDictionary,
                Func<TSnkPresentationAttribute, SnkViewModelRequest, Task<bool>> openAction,
                Func<ISnkViewModel, TSnkPresentationAttribute, Task<bool>> closeAction)
                where TSnkPresentationAttribute : class, ISnkPresentationAttribute
            {
                attributeTypesToActionsDictionary.Add(typeof(TSnkPresentationAttribute),
                    new SnkPresentationAttributeAction
                    {
                        OpenAction = (attribute,request)=> openAction(attribute as TSnkPresentationAttribute,request),
                        CloseAction = (viewModel, attribute)=> closeAction(viewModel, attribute as TSnkPresentationAttribute)
                    });
            }
        }
    }
}