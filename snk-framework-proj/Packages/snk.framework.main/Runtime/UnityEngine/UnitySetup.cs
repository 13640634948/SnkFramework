using SnkFramework.IoC;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Runtime.Core;

namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public abstract class UnitySetup : SnkSetup, IUnitySetup
        {
            protected override ISnkLoggerProvider CreateLoggerProvider()
                => new UnityLoggerProvider();


            public virtual void PlatformInitialize(IUnityApplication unityApplication)
            {
                
            }
        }

        public abstract class UnitySetup<TSnkApplication> : UnitySetup
            where TSnkApplication : MvxApplication
        {
            protected override IMvxApplication CreateApp(ISnkIoCProvider iocProvider) =>
                iocProvider.IoCConstruct<TSnkApplication>();
        }
    }
}