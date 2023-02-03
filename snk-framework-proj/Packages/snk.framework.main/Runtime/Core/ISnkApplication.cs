using System.Threading.Tasks;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime.Core
{
    public interface ISnkApplication// : IMvxViewModelLocatorCollection
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


    public abstract class SnkApplication<TParameter> : SnkApplication, ISnkApplication<TParameter>
    {
        public virtual Task<TParameter> Startup(TParameter hint)
        {
            return Task.FromResult(hint);
        }
    }
}