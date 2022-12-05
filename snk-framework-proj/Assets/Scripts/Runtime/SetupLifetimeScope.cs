using SnkFramework.Runtime.Core.Setup;
using VContainer;

namespace DefaultNamespace
{
    public class SetupLifetimeScope : SnkSetupLifetimeScope
    {
        public override void InitializePrimary(IContainerBuilder builder)
        {
            //Debug.Log("InitializePrimary-IsUnityThread:" + IsUnityThread);
        }

        public override void InitializeSecondary(IContainerBuilder builder)
        {
            //Debug.Log("InitializeSecondary-IsUnityThread:" + IsUnityThread);
        }
    }
}