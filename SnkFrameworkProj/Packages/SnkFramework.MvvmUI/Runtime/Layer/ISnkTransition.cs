using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkTransition
        {
            public SnkTransitionOperation DoTransitionTask();
        }
    }
}