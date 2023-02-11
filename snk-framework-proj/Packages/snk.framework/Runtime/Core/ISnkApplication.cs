using System.Threading.Tasks;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime.Core
{
    public interface ISnkApplication
    {
        void LoadPlugins(ISnkPluginManager pluginManager);

        void Initialize();

        Task Startup();

        void Reset();
    }
    
    public interface ISnkApplication<THint> : ISnkApplication
    {
        Task<THint> Startup(THint hint);
    }
}