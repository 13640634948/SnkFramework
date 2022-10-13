using System;
using System.Collections;
using SnkFramework.Mvvm.Core.Log;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public abstract class SnkTransition : ISnkTransition
        {
            private static readonly IMvvmLog log = SnkMvvmSetup.mMvvmLog;

            private ISnkWindowControllable window;
            private bool done = false;
            private bool animationDisabled = false;
            private int layer = 0;
            private Func<ISnkWindow, ISnkWindow, ActionType> overlayPolicy;

            private bool running = false;

            //bind the StateChange event.
            private bool bound = false;
            private Action onStart;
            private Action<ISnkWindow, WindowState> onStateChanged;
            private Action onFinish;

            public SnkTransition(ISnkWindowControllable window)
            {
                this.window = window;
            }

            ~SnkTransition()
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
                get { return this.window; }
                set { this.window = value; }
            }

            public virtual bool IsDone
            {
                get { return this.done; }
                protected set { this.done = value; }
            }

            public virtual IEnumerator WaitForDone()
            {
                while (this.IsDone == false)
                    yield return null;
            }

            public virtual bool AnimationDisabled
            {
                get { return this.animationDisabled; }
                protected set { this.animationDisabled = value; }
            }

            public virtual int Layer
            {
                get { return this.layer; }
                protected set { this.layer = value; }
            }

            public virtual Func<ISnkWindow, ISnkWindow, ActionType> OverlayPolicy
            {
                get { return this.overlayPolicy; }
                protected set { this.overlayPolicy = value; }
            }

            protected void StateChanged(object sender, WindowStateEventArgs e)
            {
                this.RaiseStateChanged((ISnkWindow) sender, e.State);
            }

            protected virtual void RaiseStart() => this.onStart?.Invoke();

            protected virtual void RaiseStateChanged(ISnkWindow window, WindowState state)
                => this.onStateChanged?.Invoke(window, state);

            protected virtual void RaiseFinished() => this.onFinish?.Invoke();

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

            //public IAwaiter GetAwaiter()
            //{
            //    return new TransitionAwaiter(this);
            //}

            public ISnkTransition DisableAnimation(bool disabled)
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

            public ISnkTransition AtLayer(int layer)
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

            public ISnkTransition Overlay(Func<ISnkWindow, ISnkWindow, ActionType> policy)
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

            public ISnkTransition OnStart(Action callback)
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

            public ISnkTransition OnStateChanged(Action<ISnkWindow, WindowState> callback)
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

            public ISnkTransition OnFinish(Action callback)
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
                yield return this.DoTransition();
                this.OnEnd();
            }

            protected abstract IEnumerator DoTransition();


        }
    }
}