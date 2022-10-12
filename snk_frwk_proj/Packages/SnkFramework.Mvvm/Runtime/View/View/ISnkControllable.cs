using System;
using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public interface ISnkControllable<TViewOwner,TLayer, TViewModel> : ISnkControllable, ISnkWindow<TViewOwner, TLayer, TViewModel>
        where TViewOwner : class, ISnkViewOwner
        where TLayer : class, ISnkUILayer
        where TViewModel : class, ISnkViewModel
    {
        
    }

    public interface ISnkControllable : ISnkWindow
    { /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreAnimation"></param>
        /// <returns></returns>
        IAsyncResult Activate(bool ignoreAnimation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreAnimation"></param>
        /// <returns></returns>
        IAsyncResult Passivate(bool ignoreAnimation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreAnimation"></param>
        /// <returns></returns>
        IAsyncResult DoShow(bool ignoreAnimation = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreAnimation"></param>
        /// <returns></returns>
        IAsyncResult DoHide(bool ignoreAnimation = false);

        /// <summary>
        /// 
        /// </summary>
        void DoDismiss();
    }
}