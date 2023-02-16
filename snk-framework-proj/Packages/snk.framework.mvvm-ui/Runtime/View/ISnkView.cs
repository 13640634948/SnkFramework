using System;
using System.Threading.Tasks;
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
            /// 视图是否激活
            /// </summary>
            public bool Activated { get; }

            event EventHandler VisibilityChanged;

            /// <summary>
            /// Triggered when the Activated's value to be changed.
            /// </summary>
            event EventHandler ActivatedChanged;
            
            //public void Create(ISnkBundle bundle);

            public Task Activate(bool animated);

            public Task Passivate(bool animated);
        }
    }
}