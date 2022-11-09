using System.Collections;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime.View
{
    public abstract class SnkView : SnkUIBehaviour, ISnkView
    {
        public virtual ISnkViewModel ViewModel { get; set; }
        public virtual bool Visibility { get; set; }
        public virtual bool Interactable { get; set; }
        public virtual bool Activated { get; set; }

        public abstract void Create(ISnkBundle bundle);

        public virtual SnkTransitionOperation Activate(bool animated)
        {
            SnkTransitionOperation operation = new SnkTransitionOperation();
            var routine = onActivateTransitioning(operation);
            if(routine != null && animated)
                StartCoroutine(routine);
            return operation;
        }

        public virtual SnkTransitionOperation Passivate(bool animated)
        {
            SnkTransitionOperation operation = new SnkTransitionOperation();
            var routine = onPassivateTransitioning(operation);
            if(routine != null && animated)
                StartCoroutine(routine);
            return operation;
        }
            
        /// <summary>
        /// 激活动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual IEnumerator onActivateTransitioning(SnkTransitionOperation operation) => default;

        /// <summary>
        /// 钝化动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual IEnumerator onPassivateTransitioning(SnkTransitionOperation operation) => default;

    }
}