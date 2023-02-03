using System.Threading;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet.Basic;
using UnityEngine;

namespace SnkFramework.Runtime.Core
{
    public abstract class SnkAppStart : ISnkAppStart
    {
        protected readonly ISnkMvvmService NavigationService;
        protected readonly ISnkApplication Application;

        private int startHasCommenced;

        protected SnkAppStart(ISnkApplication application, ISnkMvvmService navigationService)
        {
            Application = application;
            NavigationService = navigationService;
        }

        public async Task StartAsync(object? hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            var applicationHint = await ApplicationStartup(hint);
            if (applicationHint != null)
            {
                //MvxLogHost.Default?.Log(LogLevel.Trace, "Hint ignored in default MvxAppStart");
            }

            await NavigateToFirstViewModel(applicationHint);
        }

        protected abstract Task NavigateToFirstViewModel(object? hint = null);

        protected virtual async Task<object?> ApplicationStartup(object? hint = null)
        {
            await Application.Startup();
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
            Application.Reset();
        }
    }

    public class SnkAppStart<TViewModel> : SnkAppStart
        where TViewModel : class, ISnkViewModel
    {
        public SnkAppStart(ISnkApplication application, ISnkMvvmService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                Debug.Log(NavigationService);
                //var a = await NavigationService.OpenWindow<TViewModel>();
                var a = NavigationService.OpenWindow<TViewModel>();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}