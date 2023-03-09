using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkWindow : ISnkView, ISnkUINodeUnit, ISnkNavigator, ISnkContainer<ISnkPage>
        {
            public WIN_STATE WindowState { get; }

            public ISnkLayer Layer { get; }
            public Task Show(bool animated);
            public Task Hide(bool animated);
            public Task Dismiss(bool animated);
            public void AddPage(ISnkPage page);
            public TViewModel AddPage<TViewModel>();
            public Task AddPageAsync<TViewModel>();
        }
    }
}