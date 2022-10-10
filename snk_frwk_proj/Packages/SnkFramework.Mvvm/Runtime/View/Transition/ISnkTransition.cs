using System;
using System.Collections;

namespace SampleDevelop.Test
{
    public interface ISnkTransition
    {
        /// <summary>
        /// Returns  "true" if this transition finished.
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Wait for the result,suspends the coroutine execution.
        /// eg: yield return transition.WaitForDone();
        /// </summary>
        //object WaitForDone();
        public IEnumerator WaitForDone();


#if NETFX_CORE || NET_STANDARD_2_0 || NET_4_6
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //IAwaiter GetAwaiter();
#endif

        /// <summary>
        /// Disable animation
        /// </summary>
        /// <param name="disabled"></param>
        /// <returns></returns>
        ISnkTransition DisableAnimation(bool disabled);

        /// <summary>
        /// Sets the layer of the window in the window manager, 0 is the top layer.
        /// This method is only valid when showing a window.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        ISnkTransition AtLayer(int layer);

        /// <summary>
        /// Sets a processing policy. When a window is covered, hide it, dismiss it or do nothing.
        /// This method is only valid when showing a window.
        /// </summary>
        /// <example>
        /// This is an example, the default processing policy is as follows:
        /// <code>
        /// (previous,current) => {
        ///     if (previous == null || previous.WindowType == WindowType.FULL)
        ///         return ActionType.None;
        ///     if (previous.WindowType == WindowType.POPUP)    
        ///         return ActionType.Dismiss;
        ///     return ActionType.None;
        /// }
        /// </code>
        /// </example>
        /// <param name="policy"></param>
        /// <returns></returns>
        ISnkTransition Overlay(Func<ISnkWindow, ISnkWindow, ActionType> policy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ISnkTransition OnStart(Action callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ISnkTransition OnStateChanged(Action<ISnkWindow, WindowState> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ISnkTransition OnFinish(Action callback);
    }
}