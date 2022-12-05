using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters.Attributes
    {
        public interface ISnkPresentationAttribute
        {
            /// <summary>
            /// That shall be used only if you are using non generic views.
            /// </summary>
            Type? ViewModelType { get; set; }

            /// <summary>
            /// Type of the view
            /// </summary>
            Type? ViewType { get; set; }
        }
    }
}