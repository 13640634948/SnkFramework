using System;
using System.Collections;
using SnkFramework.Mvvm.Core.ViewModel;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkWindowControllable<TViewOwner, TLayer, TViewModel> : ISnkWindowControllable,
            ISnkWindow<TViewOwner, TLayer, TViewModel>
            where TViewOwner : class, ISnkViewOwner
            where TLayer : class, ISnkUILayer
            where TViewModel : class, ISnkViewModel
        {

        }

        public interface ISnkWindowControllable : ISnkWindow
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ignoreAnimation"></param>
            /// <returns></returns>
            IEnumerator Activate(bool ignoreAnimation);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ignoreAnimation"></param>
            /// <returns></returns>
            IEnumerator Passivate(bool ignoreAnimation);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ignoreAnimation"></param>
            /// <returns></returns>
            IEnumerator DoShow(bool ignoreAnimation = false);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ignoreAnimation"></param>
            /// <returns></returns>
            IEnumerator DoHide(bool ignoreAnimation = false);

            /// <summary>
            /// 
            /// </summary>
            void DoDismiss();
        }
    }
}