using System.Threading;
using MvvmCross.Core;

namespace MvvmCross.Unity.Core
{
    public class MvxUnitySetupSingleton : MvxSetupSingleton
    {
        public static MvxUnitySetupSingleton EnsureSingletonAvailable()
        {
            var instance = EnsureSingletonAvailable<MvxUnitySetupSingleton>();
            instance.PlatformSetup<MvxUnitySetup>()?.PlatformInitialize(SynchronizationContext.Current);
            return instance;
        }
    }
}