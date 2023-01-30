using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnkFramework.NuGet
{
    namespace Basic
    {
        public static class SnkObjectExtensions
        {
            public static void DisposeIfDisposable(this object thing)
            {
                if (thing is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public static class MvxDictionaryExtensions
        {
            public static IDictionary<string, object> ToPropertyDictionary(this object input)
            {
                if (input == null)
                    return new Dictionary<string, object>();

                if (input is IDictionary<string, object> dict)
                    return dict;

                var propertyInfos =
                    input.GetType().GetProperties(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                    .Where(p => p.CanRead);

                var dictionary = new Dictionary<string, object>();
                foreach (var propertyInfo in propertyInfos)
                {
                    dictionary[propertyInfo.Name] = propertyInfo.GetValue(input);
                }
                return dictionary;
            }
        }
    }
}



