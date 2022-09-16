using System;

namespace SnkFramework.Mvvm.View
{
    public interface ISnkWindowControllable
    {
        /// <summary>
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