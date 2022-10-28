using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters.Attributes
    {
        [AttributeUsage(AttributeTargets.Class)]
        public abstract class SnkBasePresentationAttribute : Attribute, ISnkPresentationAttribute
        {
            /// <inheritdoc />
            public Type? ViewModelType { get; set; }

            /// <inheritdoc />
            public Type? ViewType { get; set; }
        }
    }
}