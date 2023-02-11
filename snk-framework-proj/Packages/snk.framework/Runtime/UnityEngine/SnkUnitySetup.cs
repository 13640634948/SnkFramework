using SnkFramework.IoC;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Runtime.Core;

namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public abstract class SnkUnitySetup : SnkSetup, ISnkUnitySetup
        {
            protected override ISnkLoggerProvider CreateLoggerProvider()
                => new SnkUnityLoggerProvider();

            public virtual void PlatformInitialize()
            {
                
            }
        }

        public abstract class SnkUnitySetup<TSnkApplication> : SnkUnitySetup
            where TSnkApplication : SnkApplication
        {
            protected override ISnkApplication CreateApp(ISnkIoCProvider iocProvider) =>
                iocProvider.IoCConstruct<TSnkApplication>();
        }
    }
}