using System;
using System.Collections;
using MvvmCross.Base;
using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUIWindow<TViewModel> : MvxUGUIView<TViewModel>, IMvxUGUIWindow<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        protected EventHandler? showingCalled;
        protected EventHandler<MvxValueEventArgs<bool>>? showedCalled;

        protected EventHandler? hidingCalled;
        protected EventHandler<MvxValueEventArgs<bool>>? hiddenCalled;


        public event EventHandler? ShowingCalled
        {
            add => showingCalled += value;
            remove => showingCalled -= value;
        }

        public event EventHandler<MvxValueEventArgs<bool>>? ShowedCalled
        {
            add => showedCalled += value;
            remove => showedCalled -= value;
        }

        public event EventHandler? HidingCalled
        {
            add => hidingCalled += value;
            remove => hidingCalled -= value;
        }

        public event EventHandler<MvxValueEventArgs<bool>>? HiddenCalled
        {
            add => hiddenCalled += value;
            remove => hiddenCalled -= value;
        }

        protected virtual IEnumerator doShow()
        {
            yield break;
        }

        protected virtual IEnumerator doHide()
        {
            yield break;
        }


        public virtual IEnumerator Show(bool animated)
        {
            if (animated)
            {
                this.showingCalled?.Raise(this);
                yield return doShow();
            }

            this.showedCalled?.Raise(this, animated);
        }

        public virtual IEnumerator Hide(bool animated)
        {
            if (animated)
            {
                this.hidingCalled?.Raise(this);
                yield return doHide();
            }

            this.hiddenCalled?.Raise(this, animated);
        }
    }
}