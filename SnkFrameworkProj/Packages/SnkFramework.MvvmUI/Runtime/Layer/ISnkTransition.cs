using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkTransition
        {
            public Task<bool> DoTransitionTask();
        }
    }
}