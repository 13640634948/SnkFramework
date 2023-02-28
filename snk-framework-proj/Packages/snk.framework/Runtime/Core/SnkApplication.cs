using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Logging;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime.Core
{
    public abstract class SnkApplication<TParameter> : SnkApplication, ISnkApplication<TParameter>
    {
        public virtual Task<TParameter> Startup(TParameter hint)
        {
            return Task.FromResult(hint);
        }
    }

    public abstract class SnkApplication : ISnkApplication
    {
        public virtual void LoadPlugins(ISnkPluginManager pluginManager)
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual Task Startup()
        {
            if(SnkLogHost.Default.IsInfoEnabled)
                SnkLogHost.Default?.Info("AppStart: Application Startup - On UI thread");
            return Task.CompletedTask;
        }

        public virtual void Reset()
        {
        }

        protected void RegisterCustomAppStart<TSnkAppStart>()
            where TSnkAppStart : class, ISnkAppStart
        {
            Snk.IoCProvider?.ConstructAndRegisterSingleton<ISnkAppStart, TSnkAppStart>();
        }

        protected void RegisterAppStart<TViewModel>()
            where TViewModel : class, ISnkViewModel
        {
            Snk.IoCProvider?.ConstructAndRegisterSingleton<ISnkAppStart, SnkAppStart<TViewModel>>();
        }

        protected void RegisterAppStart(ISnkAppStart appStart)
        {
            Snk.IoCProvider?.RegisterSingleton(appStart);
        }
    }
}