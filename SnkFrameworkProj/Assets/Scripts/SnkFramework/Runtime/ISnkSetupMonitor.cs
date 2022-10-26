using System.Threading.Tasks;

namespace SnkFramework.Runtime
{
    public interface ISnkSetupMonitor
    {
        Task InitializationComplete();
    }
}