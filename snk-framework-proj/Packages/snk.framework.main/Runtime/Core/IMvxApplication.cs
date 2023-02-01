using System.Threading.Tasks;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime.Core
{
    public interface IMvxApplication// : IMvxViewModelLocatorCollection
    {
        void LoadPlugins(ISnkPluginManager pluginManager);

        void Initialize();

        Task Startup();

        void Reset();
    }
    
    public interface IMvxApplication<THint> : IMvxApplication
    {
        Task<THint> Startup(THint hint);
    }


    public abstract class MvxApplication<TParameter> : MvxApplication, IMvxApplication<TParameter>
    {
        public virtual Task<TParameter> Startup(TParameter hint)
        {
            return Task.FromResult(hint);
        }
    }
}