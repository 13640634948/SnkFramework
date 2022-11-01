using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        [RequireComponent(typeof(RectTransform), typeof(Canvas), typeof(CanvasGroup))]
        public abstract class SnkUIBehaviour : UIBehaviour
        {
            private RectTransform _rectTransform;

            public RectTransform rectTransform => _rectTransform ??= this.transform as RectTransform;

            public void SetRectTransformIdentity()
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.anchoredPosition3D = Vector3.zero;
                rectTransform.localScale = Vector3.one;
            }
        }
    }
}