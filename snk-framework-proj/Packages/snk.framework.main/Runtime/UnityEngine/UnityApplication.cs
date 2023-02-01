using UnityEngine;
using SnkFramework.Runtime.Core;
    
namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public abstract class UnityApplication : Application, IUnityApplication
        {
            public static UnityApplication Instance { get; private set; }

            protected UnityApplication()
            {
                Instance = this;
                RegisterSetup();
            }

            protected abstract void RegisterSetup();
        }

        public abstract class UnityApplication<TSnkSetup> : UnityApplication
            where TSnkSetup : SnkSetup, new()
        {
            protected override void RegisterSetup()
            {
                //this.RegisterSetupType<TMvxAndroidSetup>();
                SnkSetup.RegisterSetupType<TSnkSetup>();
            }
        }
    }
}