using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class MvxViewForAttribute : Attribute
        {
            public Type ViewModel { get; set; }

            public MvxViewForAttribute(Type viewModel)
            {
                ViewModel = viewModel;
            }
        }
    }
}