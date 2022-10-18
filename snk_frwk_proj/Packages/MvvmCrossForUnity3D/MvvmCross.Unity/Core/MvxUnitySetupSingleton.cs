using System.Threading;
using MvvmCross.Core;
using UnityEngine;

namespace MvvmCross.Unity.Core
{
    public class MvxUnitySetupSingleton : MvxSetupSingleton
    {
        public static MvxUnitySetupSingleton EnsureSingletonAvailable(SynchronizationContext synchronizationContext)
        {
            var instance = EnsureSingletonAvailable<MvxUnitySetupSingleton>();
            MvxUnitySetup setup = instance.PlatformSetup<MvxUnitySetup>();
            setup.StateChanged += (sender, args) =>
            {
                Debug.Log("SetupState:" + args.SetupState);
            };
            setup.PlatformInitialize(synchronizationContext);
            return instance;
        }
    }
}