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

            public SnkTransitionOperation Activate();

            public SnkTransitionOperation Passivate();
        }
    }
}