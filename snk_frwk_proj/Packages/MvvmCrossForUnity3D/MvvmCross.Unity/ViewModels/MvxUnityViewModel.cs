using System.Threading.Tasks;
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
    public abstract class MvxUnityViewModel<TParameter, TResult> : MvxUnityViewModel, IMvxUnityViewModel<TParameter, TResult>
    {
        public virtual void Prepare(TParameter parameter)
        {
        }

        public virtual TaskCompletionSource<object?>? CloseCompletionSource { get; set; }
    }
    
}