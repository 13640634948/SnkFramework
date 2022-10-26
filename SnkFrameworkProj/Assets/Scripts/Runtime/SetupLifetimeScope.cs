using SnkFramework.Runtime;

namespace DefaultNamespace
{
    public class SetupLifetimeScope : SnkSetupLifetimeScope
    {
        public override void InitializePrimary()
        {
            //Debug.Log("InitializePrimary-IsUnityThread:" + IsUnityThread);
        }

        public override void InitializeSecondary()
        {
            //Debug.Log("InitializeSecondary-IsUnityThread:" + IsUnityThread);
        }
    }
}