using System;
using SnkFramework.Mvvm.View;
using System.Collections;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UIAnimation : ISnkAnimation
    {
        private Action _onStart;
        private Action _onEnd;

        private ANIM_TYPE animType;
        public ANIM_TYPE AnimType
        {
            get => this.animType;
            set => this.animType = value;
        }
        
        private ISnkView _view;
        protected ISnkView mView => _view;

        public virtual void Initialize(ISnkView view)
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
                    if (this._view is ISnkWindowView)
                        (this._view as ISnkWindowView).mActivationAnimation = this;
                    break;
                case ANIM_TYPE.passivation_anim:
                    if (this._view is ISnkWindowView)
                        (this._view as ISnkWindowView).mPassivationAnimation = this;
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

        public ISnkAnimation OnStart(Action onStart)
        {
            this._onStart += onStart;
            return this;
        }

        public ISnkAnimation OnEnd(Action onEnd)
        {
            this._onEnd += onEnd;
            return this;
        }

        //public virtual void Play()
        public virtual ISnkAnimation Play()
        {
            SnkMvvmSetup.mCoroutineExecutor.RunOnCoroutineNoReturn(DoPlay());
            return default;
        }
        protected abstract IEnumerator DoPlay();

    }
}