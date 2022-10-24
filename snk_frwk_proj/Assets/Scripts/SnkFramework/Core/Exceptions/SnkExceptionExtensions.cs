using System;

namespace SnkFramework.Core.Exceptions
{
#nullable enable
    public static class SnkExceptionExtensions
    {
        public static string ToLongString(this Exception? exception)
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
#nullable restore
}