using System.Threading.Tasks;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public interface ISnkSetupMonitor
        {
            Task InitializationComplete();
        }
    }
}