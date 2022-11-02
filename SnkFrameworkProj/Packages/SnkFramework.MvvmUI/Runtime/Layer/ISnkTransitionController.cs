using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public interface ISnkTransitionController
    {
        public Task<bool> Open(ISnkWindow window);

        //public Task<bool> Hide(ISnkWindow window);
        public Task<bool> Close(ISnkWindow window);
    }
}