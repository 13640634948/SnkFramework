using System.Collections;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkView
        {
            public ISnkViewModel ViewModel { get; set; }

            /// <summary>
            /// 视图是否显示
            /// </summary>
            public bool Visibility { get; set; }

            /// <summary>
            /// 视图是否可交互
            /// </summary>
            public bool Interactable { get; set; }

            /// <summary>
            /// 视图是否激活
            /// </summary>
            public bool Activated { get; set; }

            public void Create(ISnkBundle bundle);

            public IEnumerator Activate();

            public IEnumerator Passivate();

            public void Dismiss();
        }

        public interface ISnkNavigator
        {
            public ISnkView Current { get; }
            public ISnkView NavigatorPrev { get; }
            public ISnkView NavigatorNext { get; }
        }

        public interface ISnkPage : ISnkView, ISnkNavigator, ISnkContainer<ISnkView>
        {
            public ISnkWindow ParentWindow { get; }
        }

        public interface ISnkWindow : ISnkView,  ISnkNavigator, ISnkContainer<ISnkPage>
        {
            public SnkWindowState WindowState { get; }
            public ISnkLayer Layer { get; }
            public IEnumerator Show(bool animated);
            public IEnumerator Hide(bool animated);

            public void AddPage(ISnkPage page);
            public TViewModel AddPage<TViewModel>();
            public Task<TViewModel> AddPageAsync<TViewModel>();
        }

        public interface ISnkLayer
        {
            public string LayerName { get; }

            public void AddWindow(ISnkWindow window);
            public void RemoveWindow(ISnkWindow window);

            public IEnumerator ShowTransition(ISnkWindow window);
            public IEnumerator HideTransition(ISnkWindow window);
            public IEnumerator DismissTransition(ISnkWindow window);
        }
    }
}