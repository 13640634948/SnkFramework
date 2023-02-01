using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.Plugins;

namespace SnkFramework.Runtime.Core
{
    public abstract class MvxApplication : IMvxApplication
    {
        public virtual void LoadPlugins(ISnkPluginManager pluginManager)
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual Task Startup()
        {
            //MvxLogHost.Default?.Log(LogLevel.Trace, "AppStart: Application Startup - On UI thread");
            return Task.CompletedTask;
        }

        public virtual void Reset()
        {
        }
        
        protected void RegisterCustomAppStart<TMvxAppStart>()
            where TMvxAppStart : class, ISnkAppStart
        {
            Snk.IoCProvider?.ConstructAndRegisterSingleton<ISnkAppStart, TMvxAppStart>();
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

        /*
        protected virtual void RegisterAppStart<TViewModel, TParameter>()
            where TViewModel : ISnkViewModel<TParameter> where TParameter : class
        {
            Snk.IoCProvider?.ConstructAndRegisterSingleton<ISnkAppStart, SnkAppStart<TViewModel, TParameter>>();
        }
        */
    }
}