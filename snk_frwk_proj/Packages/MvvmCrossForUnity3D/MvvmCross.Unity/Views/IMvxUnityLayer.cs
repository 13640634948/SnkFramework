using System.Collections;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityLayer
    {
        public IEnumerator ShowTransition(IMvxUnityWindow window);
        public IEnumerator HideTransition(IMvxUnityWindow window);
        public IEnumerator DismissTransition(IMvxUnityWindow window);
    }
}