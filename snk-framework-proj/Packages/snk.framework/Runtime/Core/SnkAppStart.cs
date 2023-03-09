using System;
using System.Threading;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Logging;

namespace SnkFramework.Runtime.Core
{
    public abstract class SnkAppStart : ISnkAppStart
    {
        protected Lazy<ISnkMvvmService> _mvvmService = new (() 
            => Snk.IoCProvider.Resolve<ISnkMvvmService>());

        protected Lazy<ISnkApplication> _application = new(()
            => Snk.IoCProvider.Resolve<ISnkApplication>());


        protected ISnkMvvmService mvvmService => this._mvvmService.Value;
        protected ISnkApplication application => this._application.Value;

        private int startHasCommenced;

        protected SnkAppStart()
        {
        }

        public async Task StartAsync(object? hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            var applicationHint = await ApplicationStartup(hint);
            if (applicationHint != null)
            {
                if(SnkLogHost.Default.IsInfoEnabled)
                    SnkLogHost.Default?.Info("Hint ignored in default SnkAppStart");
            }

            await NavigateToFirstViewModel(applicationHint);
        }

        protected abstract Task NavigateToFirstViewModel(object? hint = null);

        protected virtual async Task<object?> ApplicationStartup(object? hint = null)
        {
            await this.application.Startup();
            return hint;
        }

        public virtual bool IsStarted => startHasCommenced != 0;

        public virtual void ResetStart()
        {
            Reset();
            Interlocked.Exchange(ref startHasCommenced, 0);
        }

        protected virtual void Reset()
        {
            application.Reset();
        }
    }

    public class SnkAppStart<TViewModel> : SnkAppStart
        where TViewModel : class, ISnkViewModel
    {
        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                await this.mvvmService.OpenWindow<TViewModel>();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}