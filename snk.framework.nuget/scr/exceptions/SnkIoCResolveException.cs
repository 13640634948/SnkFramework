using System;
using System.Runtime.Serialization;

namespace SnkFramework.NuGet.Exceptions
{
        [Serializable]
        public class SnkIoCResolveException : SnkException
        {
            public SnkIoCResolveException()
            {
            }

            public SnkIoCResolveException(string message) : base(message)
            {
            }

            public SnkIoCResolveException(string messageFormat, params object[] messageFormatArguments)
                : base(messageFormat, messageFormatArguments)
            {
            }

            // the order of parameters here is slightly different to that normally expected in an exception
            // - but this order allows us to put string.Format in place
            public SnkIoCResolveException(Exception innerException, string messageFormat, params object[] formatArguments)
                : base(innerException, messageFormat, formatArguments)
            {
            }

            public SnkIoCResolveException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected SnkIoCResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
}