using System.Threading.Tasks;
using UnityEngine;

namespace SnkFramework.Runtime.Core.Setup
{
    public abstract class SnkSplashScreen : MonoBehaviour, ISnkSetupMonitor
    {
        public abstract Task InitializationComplete();
        private void Awake()
        {
            //SnkSetupLifetimeScope.MonitorGetter = () => this;
            //setup = gameObject.AddComponent<TSetupLifetimeScope>();
        }
    }
}