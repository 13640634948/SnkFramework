using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace DefaultNamespace
{
    public class DemoGameLauncher
    {
        [RuntimeInitializeOnLoadMethod]
        public static void bootup()
        {
            var instance = UnitySetupSingleton.EnsureSingletonAvailable<DemoSetup>(null);
            instance.EnsureInitialized();
            instance.AsyncRunAppStart();
        }
    }
}