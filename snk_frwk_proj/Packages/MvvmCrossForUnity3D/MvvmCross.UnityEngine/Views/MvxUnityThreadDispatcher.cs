using System.Threading;
using System.Threading.Tasks;
using MvvmCross.UnityEngine.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnityEngine.Views
{
    public class MvxUnityThreadDispatcher : MvxUnityThreadAsyncDispatcher , IMvxViewDispatcher
    {
        private readonly IMvxUnityViewPresenter _presenter;

        public MvxUnityThreadDispatcher(SynchronizationContext synchronizationContext, IMvxUnityViewPresenter presenter) : base(synchronizationContext)
        {
            _presenter = presenter;
        }
        
        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}