using UnityEngine;

namespace SnkFramework.Mvvm.Extensions
{
    public static class RectTransformExtension
    {
        public static void Identity(this RectTransform rectTransform)
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