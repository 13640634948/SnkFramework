using System.Threading.Tasks;

namespace SnkFramework.Runtime.Core.Setup
{
    public interface ISnkSetupMonitor
    {
        Task InitializationComplete();
    }
}