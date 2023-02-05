using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class SnkViewForAttribute : Attribute
        {
            public Type ViewModel { get; set; }

            public SnkViewForAttribute(Type viewModel)
            {
                ViewModel = viewModel;
            }
        }
    }
}