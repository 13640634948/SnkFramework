using System.Threading.Tasks;
using UnityEngine;

namespace SnkFramework.Runtime.Core.Setup
{
    public abstract class SnkSplashScreen<TSetupLifetimeScope> : MonoBehaviour, ISnkSetupMonitor
        where TSetupLifetimeScope : SnkSetupLifetimeScope
    {
        public abstract Task InitializationComplete();

        protected SnkSetupLifetimeScope setup;
        private void Awake()
        {
            SnkSetupLifetimeScope.MonitorGetter = () => this;
            setup = gameObject.AddComponent<TSetupLifetimeScope>();
        }
    }
}