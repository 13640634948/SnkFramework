using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Core
{
    public interface ISnkSetupMonitor
    {
        Task InitializationComplete();
    }
}