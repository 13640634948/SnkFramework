using System;
using SnkFramework.Mvvm.ViewModel;
using System.Collections;

namespace SnkFramework.Mvvm.View
{
    public interface ISnkWindowControllable<TViewModel> : ISnkWindowControllable, IWindow<TViewModel>
        where TViewModel : class, IViewModel, new()
    {
    }

    public interface ISnkWindowControllable : IWindow
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