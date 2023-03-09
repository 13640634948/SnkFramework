using System;
using System.Threading.Tasks;
using SnkFramework.IoC;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public class SnkMainThreadAsyncDispatcher : ISnkMainThreadAsyncDispatcher
        {
            private Lazy<ISnkMainThread> mainThread = new (()=> 
                SnkSingleton<ISnkIoCProvider>.Instance.Resolve<ISnkMainThread>());

            private ISnkMainThread MainThread => mainThread.Value;
            
            public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
            {
                if (action == null)
                    return Task.CompletedTask;

                var asyncAction = new Func<Task>(() =>
                {
                    action();
                    return Task.CompletedTask;
                });
                return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
            }

            public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
            {
                if (action == null)
                    return;

                var completion = new TaskCompletionSource<bool>();
                var syncAction = new Action(async () =>
                {
                    await action();
                    completion.SetResult(true);
                });
                ExecuteOnMainThread(syncAction, maskExceptions);

                // If we're already on main thread, then the action will
                // have already completed at this point, so can just return
                if (completion.Task.IsCompleted)
                    return;

                // Make sure we don't introduce weird locking issues  
                // blocking on the completion source by jumping onto
                // a new thread to wait
                await Task.Run(async () => await completion.Task);
            }

            public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
            {
                
                if (IsOnMainThread)
                    ExceptionMaskedAction(action, maskExceptions);
                else
                {
                    MainThread.Context.Post(ignored =>
                    {
                        ExceptionMaskedAction(action, maskExceptions);
                    }, null);
                }
                return true;
            }

            public virtual bool IsOnMainThread => MainThread.IsMainThread;

            public virtual void ExecuteOnMainThread(Action action, bool maskExceptions = true)
            {
                if (IsOnMainThread)
                    ExceptionMaskedAction(action, maskExceptions);
                else
                    MainThread.Context.Post(ignored => ExceptionMaskedAction(action, maskExceptions), null);
            }

            public   void ExceptionMaskedAction(Action action, bool maskExceptions)
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                action();
            }
        }
    }
}