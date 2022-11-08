using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public interface ISnkTransitionController
    {
        public SnkTransitionOperation Open(ISnkWindow window);

        public SnkTransitionOperation Close(ISnkWindow window);
    }
}