using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Demo
{
    public class DemoViewModel : MvxUnityViewModel
    {
        public DemoViewModel()
        {
            UnityEngine.Debug.Log("DemoViewModel.ctor");
        }
    }
}