using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.NuGet.Exceptions;

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

        public static class MvxExceptionExtensions
        {
            public static string ToLongString(this Exception exception)
            {
                if (exception == null)
                    return "null exception";

                if (exception.InnerException != null)
                {
                    var innerExceptionText = exception.InnerException.ToLongString();
                    return
                        $"{exception.GetType().Name}: {exception.Message ?? "-"}\n\t{exception.StackTrace}\nInnerException was {innerExceptionText}";
                }
                return $"{exception.GetType().Name}: {exception.Message ?? "-"}\n\t{exception.StackTrace}";
            }

            public static Exception MvxWrap(this Exception exception)
            {
                if (exception is SnkException)
                    return exception;

                return MvxWrap(exception, exception.Message);
            }

            public static Exception MvxWrap(this Exception exception, string message)
            {
                return new SnkException(exception, message);
            }

            public static Exception MvxWrap(this Exception exception, string messageFormat, params object[] formatArguments)
            {
                return new SnkException(exception, messageFormat, formatArguments);
            }
        }
    }
}



