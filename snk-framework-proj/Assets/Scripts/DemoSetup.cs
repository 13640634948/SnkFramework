using SnkFramework.Mvvm.Demo.Implements;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Runtime.Engine;

namespace DefaultNamespace
{
    public class DemoSetup : UnitySetup<DemoApp>
    {
        protected override ISnkMvvmInstaller CreateMvvmInstaller()
            => new DemoMvvmInstaller();
    }
}