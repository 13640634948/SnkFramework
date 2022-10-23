using MvvmCross.Unity.Views;
using MvvmCross.Unity.Views.Base;
using MvvmCross.Unity.Views.UGUI;

namespace MvvmCross.Demo
{
    public class DemoFullWindow : MvxUGUIWindow<DemoViewModel>
    {
        public DemoFullWindow()
        {
            UnityEngine.Debug.Log("DemoView.ctor");
        }

        public override void Created(MvxUnityBundle bundle)
        {
            base.Created(bundle);
            UnityEngine.Debug.Log("DemoFullWindow.Created");
        }

        public override void Appearing()
        {
            base.Appearing();
            UnityEngine.Debug.Log("DemoFullWindow.Appearing");
        }

        public override void Appeared(IMvxUnityOwner unityOwner)
        {
            base.Appeared(unityOwner);
            UnityEngine.Debug.Log("DemoFullWindow.Appeared");
        }
    }
}