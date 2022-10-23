using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract partial class MvxUGUIWindow<TViewModel> : MvxUGUIView<TViewModel>, IMvxUGUIWindow<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        IMvxUnityLayer IMvxUnityWindow.Layer
        {
            get => Layer;
            set => Layer = value as IMvxUGUILayer;
        }

        public IMvxUGUILayer Layer { get; set; }
    }
}