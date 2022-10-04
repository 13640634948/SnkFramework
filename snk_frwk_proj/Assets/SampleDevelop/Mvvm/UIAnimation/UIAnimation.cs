using System;
using SnkFramework.Mvvm.View;
using System.Collections;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UIAnimation : IAnimation
    {
        private Action _onStart;
        private Action _onEnd;

        private ANIM_TYPE animType;
        public ANIM_TYPE AnimType
        {
            get => this.animType;
            set => this.animType = value;
        }
        
        private IView _view;
        protected IView mView => _view;

        public virtual void Initialize(IView view)
        {
            this._view = view;
            switch (this.AnimType)
            {
                case ANIM_TYPE.enter_anim:
                    this._view.mEnterAnimation = this;
                    break;
                case ANIM_TYPE.exit_anim:
                    this._view.mExitAnimation = this;
                    break;
                case ANIM_TYPE.activation_anim:
                    if (this._view is IWindowView)
                        (this._view as IWindowView).mActivationAnimation = this;
                    break;
                case ANIM_TYPE.passivation_anim:
                    if (this._view is IWindowView)
                        (this._view as IWindowView).mPassivationAnimation = this;
                    break;
            }
        }

        protected void OnStart()
        {
            try
            {
                this._onStart.Invoke();
                this._onStart = null;
            }
            catch (Exception) { }
        }

        protected void OnEnd()
        {
            try
            {
                this._onEnd?.Invoke();
                this._onEnd = null;
            }
            catch (Exception) { }
        }

        public IAnimation OnStart(Action onStart)
        {
            this._onStart += onStart;
            return this;
        }

        public IAnimation OnEnd(Action onEnd)
        {
            this._onEnd += onEnd;
            return this;
        }

        public virtual void Play()
        {
            SnkMvvmSetup.mCoroutineExecutor.RunOnCoroutineNoReturn(DoPlay());
        }

        protected abstract IEnumerator DoPlay();

    }
}