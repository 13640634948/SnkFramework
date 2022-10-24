using System.Collections;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUILayer : MvxUGUINode, IMvxUnityLayer
    {
        public IEnumerator ShowTransition(IMvxUnityWindow window, bool animated)
        {
            yield return window.Activate(animated);
        }

        public IEnumerator HideTransition(IMvxUnityWindow window, bool animated)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator DismissTransition(IMvxUnityWindow window, bool animated)
        {
            throw new System.NotImplementedException();
        }

        public void Add(IMvxUnityWindow window)
        {
            IMvxUGUIWindow uguiWindow = window as IMvxUGUIWindow;
            uguiWindow.Owner.transform.SetParent(this.transform);
        }

        public void Remove(IMvxUnityWindow window)
        {
            throw new System.NotImplementedException();
        }
    }
}