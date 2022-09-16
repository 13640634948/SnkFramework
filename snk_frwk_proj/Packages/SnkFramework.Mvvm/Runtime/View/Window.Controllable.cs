using System;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public abstract partial class Window<TViewModel>
    {
        
        public IAsyncResult Activate(bool ignoreAnimation)
        {
            if (this.mVisibility == false)
                this.mVisibility = true;
            if (this.Activated == false)
            {
                this.Activated = true;
                this.State = WindowState.ACTIVATED;
            }

            return default;
        }

        public IAsyncResult Passivate(bool ignoreAnimation)
        {
            this.Activated = false;
            this.State = WindowState.PASSIVATED;

            return default;
        }

        public IAsyncResult DoShow(bool ignoreAnimation = false)
        {
            if (this._created == false)
                this.Create();
            this.onShow();
            this.mVisibility = true;
            this.State = WindowState.VISIBLE;
            return default;
        }

        public IAsyncResult DoHide(bool ignoreAnimation = false)
        {
            this.mVisibility = false;
            this.State = WindowState.INVISIBLE;
            this.onHide();
            return default;
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
                    //this.WindowManager.Remove(this);

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