using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Demo
{
    public class DemoViewModel : MvxUnityViewModel
    {
        public DemoViewModel()
        {
            UnityEngine.Debug.Log("DemoViewModel.ctor");
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            UnityEngine.Debug.Log("DemoViewModel.ViewCreated");
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            UnityEngine.Debug.Log("DemoViewModel.ViewAppearing");
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            UnityEngine.Debug.Log("DemoViewModel.ViewAppeared");
        }
    }
}