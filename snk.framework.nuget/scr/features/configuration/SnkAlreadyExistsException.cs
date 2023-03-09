using System;

namespace SnkFramework.NuGet.Features
{
    namespace Configuration
    {
        public class SnkAlreadyExistsException : Exception
        {
            public SnkAlreadyExistsException()
            {
            }

            public SnkAlreadyExistsException(string message) : base(message)
            {
            }

            public SnkAlreadyExistsException(Exception exception) : base("", exception)
            {
            }

            public SnkAlreadyExistsException(string message, Exception exception) : base(message, exception)
            {
            }
        }
    }
}
