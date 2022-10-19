using MvvmCross.Unity.Views.UGUI;

namespace MvvmCross.Demo
{
    public class DemoView : MvxUGUIFullWindow<DemoViewModel>
    {
        public DemoView()
        {
            UnityEngine.Debug.Log("DemoView.ctor");
        }
    }
}