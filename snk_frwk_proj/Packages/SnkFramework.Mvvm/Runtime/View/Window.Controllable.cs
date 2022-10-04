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

            if (this.mActivated == true)
                yield break;


            this.mActivated = true;
            this.State = WIN_STATE.activation;
            if (ignoreAnimation == false && this.mPassivationAnimation != null)
            {
                bool animCompleted = false;
                this.mActivationAnimation.OnStart(() => { this.State = WIN_STATE.activation_anim_begin; }).OnEnd(
                    () =>
                    {
                        this.State = WIN_STATE.activation_anim_end;
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
            this.State = WIN_STATE.passivation;

            if (ignoreAnimation == false && this.mPassivationAnimation != null)
            {
                bool animCompleted = false;
                this.mPassivationAnimation
                    .OnStart(() => this.State = WIN_STATE.passivation_anim_begin)
                    .OnEnd(() =>
                    {
                        this.State = WIN_STATE.passivation_anim_end;
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
            this.State = WIN_STATE.visible;

            if (ignoreAnimation == false && this.mEnterAnimation != null)
            {
                log.InfoFormat("[{0}]EnterAnimation-0", Time.frameCount);
                bool animCompleted = false;
                this.mEnterAnimation
                    .OnStart(() => this.State = WIN_STATE.enter_anim_begin)
                    .OnEnd(() =>
                    {
                        this.State = WIN_STATE.enter_anim_end;
                        animCompleted = true;
                        log.InfoFormat("[{0}]EnterAnimation-1", Time.frameCount);
                    })
                    .Play();
                log.InfoFormat("[{0}]EnterAnimation-2", Time.frameCount);
                yield return new WaitUntil(() => animCompleted);
                log.InfoFormat("[{0}]EnterAnimation-3", Time.frameCount);
            }
        }

        public IEnumerator DoHide(bool ignoreAnimation = false)
        {
            bool animCompleted = false;
            if (ignoreAnimation == false && this.mExitAnimation != null)
            {
                this.mExitAnimation.OnStart(() => this.State = WIN_STATE.exit_anim_begin)
                    .OnEnd(() =>
                    {
                        this.State = WIN_STATE.exit_anim_end;
                        this.mVisibility = false;
                        this.State = WIN_STATE.invisible;
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
                    this.State = WIN_STATE.dismiss_begin;
                    this._dismissed = true;
                    this.onDismiss();
                    this.raiseOnDismissed();
                    this.UILayer.Remove(this);

                    if (this.ownerValidityCheck())
                        GameObject.Destroy(this.mOwner.gameObject);
                    this.State = WIN_STATE.dismiss_end;
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