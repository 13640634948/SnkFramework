using System.Reflection;
using System.Threading.Tasks;
using SnkFramework.Runtime.Core;

namespace SnkFramework.Runtime.Engine
{
    public class UnitySetupSingleton : SnkSetupSingleton
    {
        public static UnitySetupSingleton EnsureSingletonAvailable<TSnkSetup>(params Assembly[] assemblies)
            where TSnkSetup : SnkSetup, new()
        {
            SnkSetup.RegisterSetupType<TSnkSetup>(assemblies);
            var instance = SnkSetupSingleton.EnsureSingletonAvailable<UnitySetupSingleton>();
            instance.PlatformSetup<UnitySetup>()?.PlatformInitialize();
            return instance;
        }

        public virtual async Task AsyncRunAppStart()
            => await Snk.IoCProvider.Resolve<ISnkAppStart>()?.StartAsync();
    }
}