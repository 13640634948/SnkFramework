using System.Collections;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityLayer
    {
        public IEnumerator ShowTransition(IMvxUnityWindow window, bool animated);
        public IEnumerator HideTransition(IMvxUnityWindow window, bool animated);
        public IEnumerator DismissTransition(IMvxUnityWindow window, bool animated);

        public void Add(IMvxUnityWindow window);
        public void Remove(IMvxUnityWindow window);
    }
}