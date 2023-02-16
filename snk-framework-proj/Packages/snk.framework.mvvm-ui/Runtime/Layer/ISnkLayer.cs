using System.Threading.Tasks;
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
            public bool RemoveChild(SnkWindow window);
            
            public Task Open(ISnkWindow window);

            public Task Close(ISnkWindow window);
        }
    }
}