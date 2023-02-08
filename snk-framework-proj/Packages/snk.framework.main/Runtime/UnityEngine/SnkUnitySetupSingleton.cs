using System.Reflection;
using System.Threading.Tasks;
using SnkFramework.Runtime.Core;

namespace SnkFramework.Runtime.Engine
{
    public class SnkUnitySetupSingleton : SnkSetupSingleton
    {
        public static async Task<SnkUnitySetupSingleton> EnsureSingletonAvailable<TSnkSetup>(params Assembly[] assemblies)
            where TSnkSetup : SnkSetup, new()
        {
            SnkSetup.RegisterSetupType<TSnkSetup>(assemblies);
            var instance = SnkSetupSingleton.EnsureSingletonAvailable<SnkUnitySetupSingleton>();
            instance.PlatformSetup<SnkUnitySetup>()?.PlatformInitialize();
            return instance;
        }

        public virtual async Task AsyncRunAppStart()
            => await Snk.IoCProvider.Resolve<ISnkAppStart>()?.StartAsync();
    }
}