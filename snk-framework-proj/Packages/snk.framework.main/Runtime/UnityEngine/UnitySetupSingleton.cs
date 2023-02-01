using System.Threading.Tasks;
using SnkFramework.Runtime.Core;

namespace SnkFramework.Runtime.Engine
{
    public class UnitySetupSingleton : SnkSetupSingleton
    {
        public static UnitySetupSingleton EnsureSingletonAvailable<TMvxSetup>(IUnityApplication unityApplication)
            where TMvxSetup : SnkSetup, new()
        {
            SnkSetup.RegisterSetupType<TMvxSetup>();
            var instance = EnsureSingletonAvailable<UnitySetupSingleton>();
            instance.PlatformSetup<UnitySetup>()?.PlatformInitialize(unityApplication);
            return instance;
        }

        public virtual async Task AsyncRunAppStart()
            => await Snk.IoCProvider.Resolve<ISnkAppStart>()?.StartAsync();
    }
}