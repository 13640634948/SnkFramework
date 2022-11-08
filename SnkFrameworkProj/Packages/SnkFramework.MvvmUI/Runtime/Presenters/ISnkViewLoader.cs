using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Presenters
    {
        public interface ISnkViewLoader
        {
            Task<SnkWindow> CreateView(SnkViewModelRequest request);

            Task<SnkWindow> CreateView(Type viewType);

            bool UnloadView(SnkWindow window);
        }
    }
}