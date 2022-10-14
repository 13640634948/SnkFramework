using System.Threading;
using SnkFramework.Mvvm.Core;
using UnityEngine;

namespace SampleDevelop.Mvvm
{
    public class SnkDemoSetupSingleton
        : MvxSetupSingleton
    {
        public static SnkDemoSetupSingleton EnsureSingletonAvailable(SynchronizationContext applicationContext)
        {
            var instance = EnsureSingletonAvailable<SnkDemoSetupSingleton>();
            instance.PlatformSetup<SnkDemoMvvmSetup>()?.PlatformInitialize(applicationContext);
            return instance;
        }
    }
}