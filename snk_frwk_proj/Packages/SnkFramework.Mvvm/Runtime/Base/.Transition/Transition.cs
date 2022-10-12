using System;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.View;
using System.Collections;

namespace SnkFramework.Mvvm.Base
{
    public abstract class Transition : ITransition
    {
        private static readonly IMvvmLog log = SnkMvvmSetup.mMvvmLog;

        private ISnkWindowControllable window;
        private bool done = false;
        private bool animationDisabled = false;
        private int layer = 0;
        private Func<IWindow, IWindow, ActionType> overlayPolicy;

        private bool running = false;

        private bool bound = false;
        private Action onStart;
        private Action<IWindow, WIN_STATE> onStateChanged;
        private Action onFinish;

        public Transition(ISnkWindowControllable window)
        {
            this.window = window;
        }

        ~Transition()
        {
            this.Unbind();
        }

        protected virtual void Bind()
        {
            if (bound)
                return;

            this.bound = true;
            if (this.window != null)
                this.window.StateChanged += StateChanged;
        }

        protected virtual void Unbind()
        {
            if (!bound)
                return;

            this.bound = false;

            if (this.window != null)
                this.window.StateChanged -= StateChanged;
        }

        public virtual ISnkWindowControllable Window
        {
            get => this.window;
            set => this.window = value;
        }

        public virtual bool IsDone
        {
            get => this.done;
            protected set => this.done = value;
        }

        public virtual object WaitForDone() => this.IsDone;

        public virtual bool AnimationDisabled
        {
            get => this.animationDisabled;
            protected set => this.animationDisabled = value;
        }

        public virtual int Layer
        {
            get => this.layer;
            protected set => this.layer = value;
        }

        public virtual Func<IWindow, IWindow, ActionType> OverlayPolicy
        {
            get => this.overlayPolicy;
            protected set => this.overlayPolicy = value;
        }

        protected void StateChanged(object sender, WindowStateEventArgs e)
        {
            this.RaiseStateChanged((IWindow) sender, e.State);
        }

        protected virtual void RaiseStart()
        {
            try
            {
                if (this.onStart != null)
                    this.onStart();
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected virtual void RaiseStateChanged(IWindow window, WIN_STATE state)
        {
            try
            {
                if (this.onStateChanged != null)
                    this.onStateChanged(window, state);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected virtual void RaiseFinished()
        {
            try
            {
                if (this.onFinish != null)
                    this.onFinish();
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("", e);
            }
        }

        protected virtual void OnStart()
        {
            this.Bind();
            this.RaiseStart();
        }

        protected virtual void OnEnd()
        {
            this.done = true;
            this.RaiseFinished();
            this.Unbind();
        }

        /*
        public IAwaiter GetAwaiter()
        {
            return new TransitionAwaiter(this);
        }
        */

        public ITransition DisableAnimation(bool disabled)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.DisableAnimation failed.");

                return this;
            }

            this.animationDisabled = disabled;
            return this;
        }

        public ITransition AtLayer(int layer)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.sets the layer failed.");

                return this;
            }

            this.layer = layer;
            return this;
        }

        public ITransition Overlay(Func<IWindow, IWindow, ActionType> policy)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.sets the policy failed.");

                return this;
            }

            this.OverlayPolicy = policy;
            return this;
        }

        public ITransition OnStart(Action callback)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.OnStart failed.");

                return this;
            }

            this.onStart += callback;
            return this;
        }

        public ITransition OnStateChanged(Action<IWindow, WIN_STATE> callback)
        {
            if (this.running)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The transition is running.OnStateChanged failed.");

                return this;
            }

            this.onStateChanged += callback;
            return this;
        }

        public ITransition OnFinish(Action callback)
        {
            if (this.done)
            {
                callback();
                return this;
            }

            this.onFinish += callback;
            return this;
        }

        public virtual IEnumerator TransitionTask()
        {
            this.running = true;
            this.OnStart();
#if UNITY_5_3_OR_NEWER
            yield return this.DoTransition();
#else
            var transitionAction = this.DoTransition();
            while (transitionAction.MoveNext())
                yield return transitionAction.Current;
#endif
            this.OnEnd();
        }

        protected abstract IEnumerator DoTransition();
    }
}