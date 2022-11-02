using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public abstract class SnkTransition : ISnkTransition
    {
        protected ISnkWindow window;

        public SnkTransition(ISnkWindow window)
        {
            this.window = window;
        }

        public Task<bool> DoTransitionTask()
        {
            return Task.FromResult(true);
        }
    }
}