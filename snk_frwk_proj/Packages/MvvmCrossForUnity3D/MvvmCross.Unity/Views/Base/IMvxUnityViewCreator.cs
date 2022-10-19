using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityViewCreator
    {
        IMvxUnityView CreateView(MvxViewModelRequest request);
    }
}