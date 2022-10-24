using System.Collections;
using UnityEngine;

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
            RectTransform child = uguiWindow.Owner.transform as RectTransform;
            child.SetParent(this.transform);
            
            child.anchorMin = Vector2.zero;
            child.anchorMax = Vector2.one;
            child.offsetMin = Vector2.zero;
            child.offsetMax = Vector2.zero;
            child.anchoredPosition3D = Vector3.zero;
            child.localScale = Vector3.one;
        }

        public void Remove(IMvxUnityWindow window)
        {
            throw new System.NotImplementedException();
        }
    }
}