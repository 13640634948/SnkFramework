using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public class SnkTransition : ISnkTransition
    {
        protected ISnkWindow window;

        public SnkTransition(ISnkWindow window)
        {
            this.window = window;
        }

        public SnkTransitionOperation DoTransitionTask()
        {
            return new SnkTransitionOperation();
        }
    }
}