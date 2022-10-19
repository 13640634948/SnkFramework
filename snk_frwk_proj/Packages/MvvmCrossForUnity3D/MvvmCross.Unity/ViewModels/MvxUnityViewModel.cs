using MvvmCross.ViewModels;

namespace MvvmCross.Unity.ViewModels
{
    public abstract class MvxUnityViewModel : MvxViewModel, IMvxUnityViewModel
    {
        public virtual void ViewShowing(bool animated)
        {
        }

        public virtual void ViewShowed()
        {
        }

        public virtual void ViewHideing(bool animated)
        {
        }

        public virtual void ViewHidden()
        {
        }
    }
}