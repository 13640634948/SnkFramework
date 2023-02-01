using System;
using SnkFramework.Mvvm.Demo.Views;
using SnkFramework.Mvvm.Runtime.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Demo
{
    namespace Implements
    {
        public class DemoViewFinder : SnkViewFinder
        {
            public override Type GetViewType(Type viewModelType)
            {
                return typeof(TestWindow);
            }
        }
    }
}