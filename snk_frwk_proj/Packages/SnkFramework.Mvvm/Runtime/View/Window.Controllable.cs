using System;
using UnityEngine;
using System.Collections;

namespace SnkFramework.Mvvm.View
{
    public abstract partial class Window<TViewModel>
    {
        public IEnumerator Activate(bool ignoreAnimation)
        {
            if (this.mVisibility == false)
                throw new InvalidOperationException("the window is not visible.");

            if (this.mActivated == false)
                yield break;


            this.mActivated = true;
            this.State = WindowState.ACTIVATED;
            if (ignoreAnimation == false && this.PassivationAnimation != null)
            {
                bool animCompleted = false;
                this.ActivationAnimation.OnStart(() => { this.State = WindowState.ACTIVATION_ANIMATION_BEGIN; }).OnEnd(
                    () =>
                    {
                        this.State = WindowState.ACTIVATION_ANIMATION_END;
                        animCompleted = true;
                    }).Play();
                yield return new WaitUntil(() => animCompleted);
            }
        }

        public IEnumerator Passivate(bool ignoreAnimation)
        {
            if (this.mVisibility == false)
                throw new InvalidOperationException("the window is not visible.");

            if (this.mActivated == false)
                yield break;

            this.mActivated = false;
            this.State = WindowState.PASSIVATED;

            if (ignoreAnimation == false && this.PassivationAnimation != null)
            {
                bool animCompleted = false;
                this.PassivationAnimation
                    .OnStart(() => this.State = WindowState.PASSIVATION_ANIMATION_BEGIN)
                    .OnEnd(() =>
                    {
                        this.State = WindowState.PASSIVATION_ANIMATION_END;
                        animCompleted = true;
                    }).Play();
                yield return new WaitUntil(() => animCompleted);
            }
        }

        public IEnumerator DoShow(bool ignoreAnimation = false)
        { 
            if(this._created == false)
                this.Create();
            this.onShow();
            
            this.mVisibility = true;
            this.State = WindowState.VISIBLE;

            if (ignoreAnimation == false && this.mEnterAnimation != null)
            {
                bool animCompleted = false;
                this.mEnterAnimation
                    .OnStart(() => this.State = WindowState.ENTER_ANIMATION_BEGIN)
                    .OnEnd(() =>
                    {
                        this.State = WindowState.ENTER_ANIMATION_END;
                        animCompleted = true;
                    })
                    .Play();
                yield return new WaitUntil(() => animCompleted);
            }
        }

        public IEnumerator DoHide(bool ignoreAnimation = false)
        {
            bool animCompleted = false;
            if (ignoreAnimation == false && this.mExitAnimation != null)
            {
                this.mExitAnimation.OnStart(() => this.State = WindowState.EXIT_ANIMATION_BEGIN)
                    .OnEnd(() =>
                    {
                        this.State = WindowState.EXIT_ANIMATION_END;
                        this.mVisibility = false;
                        this.State = WindowState.INVISIBLE;
                        this.onHide();
                        animCompleted = true;
                    }).Play();
                yield return new WaitUntil(() => animCompleted);
            }
        }

        public virtual void DoDismiss()
        {
            try
            {
                if (!this._dismissed)
                {
                    this.State = WindowState.DISMISS_BEGIN;
                    this._dismissed = true;
                    this.onDismiss();
                    this.raiseOnDismissed();
                    this.UILayer.Remove(this);

                    if (this.ownerValidityCheck())
                        GameObject.Destroy(this.mOwner.gameObject);
                    this.State = WindowState.DISMISS_END;
                    this._dismissTransition = null;
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("The window named \"{0}\" failed to dismiss!Error:{1}", this.mName, e);
            }
        }
    }
}