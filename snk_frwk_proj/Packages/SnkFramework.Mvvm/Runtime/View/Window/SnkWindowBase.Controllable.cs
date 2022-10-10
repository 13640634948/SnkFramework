using System;
using System.Threading;

namespace SampleDevelop.Test
{
    internal class UIAsyncResult : IAsyncResult
    {
        private bool _isCompleted = false;
        private Exception _exception = null;

        public object AsyncState { get; }
        public WaitHandle AsyncWaitHandle { get; }
        public bool CompletedSynchronously { get; }

        public bool IsCompleted => _isCompleted;
        private Exception mException => _exception;

        public void SetException(Exception exception)
        {
            this._exception = exception;
            this.SetResult();
        }

        public void SetResult()
        {
            _isCompleted = true;
        }
    }


    public abstract partial class SnkWindowBase : ISnkWindowControllable
    {
        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnDismiss()
        {
        }


        public ISnkTransition Show(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new InvalidOperationException("The window has been destroyed");

            if (this.mVisibility)
                throw new InvalidOperationException("The window is already visible.");

            return this.mUILayer.Show(this).DisableAnimation(ignoreAnimation);
        }

        public ISnkTransition Hide(bool ignoreAnimation = false)
        {
            if (!this._created)
                throw new InvalidOperationException("The window has not been created.");

            if (this._dismissed)
                throw new InvalidOperationException("The window has been destroyed.");

            if (!this.mVisibility)
                throw new InvalidOperationException("The window is not visible.");

            return this.mUILayer.Hide(this).DisableAnimation(ignoreAnimation);
        }

        public ISnkTransition Dismiss(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null)
                return this._dismissTransition;

            if (this._dismissed)
                throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", this.mName));

            this._dismissTransition = this.mUILayer.Dismiss(this).DisableAnimation(ignoreAnimation);
            return this._dismissTransition;
        }


        public IAsyncResult Activate(bool ignoreAnimation)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this.mVisibility == false)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (this.mActivated)
                {
                    result.SetResult();
                    return result;
                }

                if (!ignoreAnimation && this.mActivationAnimation != null)
                {
                    this.mActivationAnimation.OnStart(() =>
                    {
                        this.mWindowState = WindowState.ACTIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        this.mWindowState = WindowState.ACTIVATION_ANIMATION_END;
                        this.mActivated = true;
                        this.mWindowState = WindowState.ACTIVATED;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    this.mActivated = true;
                    this.mWindowState = WindowState.ACTIVATED;
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }

            return result;
        }

        public IAsyncResult Passivate(bool ignoreAnimation)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this.mVisibility == false)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (this.mActivated == false)
                {
                    result.SetResult();
                    return result;
                }

                this.mActivated = false;
                this.mWindowState = WindowState.PASSIVATED;

                if (!ignoreAnimation && this.mPassivationAnimation != null)
                {
                    this.mPassivationAnimation.OnStart(() =>
                    {
                        this.mWindowState = WindowState.PASSIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        this.mWindowState = WindowState.PASSIVATION_ANIMATION_END;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }

            return result;
        }


        public IAsyncResult DoShow(bool ignoreAnimation = false)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this._created == false)
                {
                    new System.Exception("未成功创建");
                }

                this.OnShow();
                this.mVisibility = true;
                this.mWindowState = WindowState.VISIBLE;
                if (!ignoreAnimation && this.mEnterAnimation != null)
                {
                    this.mEnterAnimation.OnStart(() => { this.mWindowState = WindowState.ENTER_ANIMATION_BEGIN; })
                        .OnEnd(() =>
                        {
                            this.mWindowState = WindowState.ENTER_ANIMATION_END;
                            result.SetResult();
                        }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
                UnityEngine.Debug.LogException(e);
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("The window named \"{0}\" failed to open!Error:{1}", this.Name, e);
            }

            return result;
        }

        public IAsyncResult DoHide(bool ignoreAnimation = false)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (!ignoreAnimation && this.mExitAnimation != null)
                {
                    this.mExitAnimation.OnStart(() => { this.mWindowState = WindowState.EXIT_ANIMATION_BEGIN; }).OnEnd(
                        () =>
                        {
                            this.mWindowState = WindowState.EXIT_ANIMATION_END;
                            this.mVisibility = false;
                            this.mWindowState = WindowState.INVISIBLE;
                            this.OnHide();
                            result.SetResult();
                        }).Play();
                }
                else
                {
                    this.mVisibility = false;
                    this.mWindowState = WindowState.INVISIBLE;
                    this.OnHide();
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("The window named \"{0}\" failed to hide!Error:{1}", this.Name, e);
            }

            return result;
        }

        public virtual void DoDismiss()
        {
            try
            {
                if (this._dismissed == false)
                {
                    this.mWindowState = WindowState.DISMISS_BEGIN;
                    this._dismissed = true;
                    this.OnDismiss();
                    this.raiseOnDismissed();

                    this.mUILayer.Remove(this);

                    this.Unload();
                    //if (!this.IsDestroyed() && this.gameObject != null)
                    //    GameObject.Destroy(this.gameObject);

                    this.mWindowState = WindowState.DISMISS_END;
                    this._dismissTransition = null;
                }
            }
            catch (Exception e)
            {
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("The window named \"{0}\" failed to dismiss!Error:{1}", this.Name, e);
            }
        }
        
    }
}