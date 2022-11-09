using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkLayer : ISnkBehaviourOwner, ISnkUINodeUnit
        {
            public string LayerName { get; }
            public void AddChild(SnkWindow window);
            public SnkWindow GetChild(int index);

            public SnkTransitionOperation Open(ISnkWindow window);

            public SnkTransitionOperation Close(ISnkWindow window);
        }
    }
}