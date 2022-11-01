using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Runtime.Layer
{
    public interface ISnkTransition
    {
        public Task<bool> DoTransitionTask();
    }
}