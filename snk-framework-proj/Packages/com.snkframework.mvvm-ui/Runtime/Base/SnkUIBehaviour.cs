using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        [RequireComponent(typeof(RectTransform))]
        public abstract class SnkUIBehaviour : UIBehaviour
        {
            private RectTransform _rectTransform;

            public RectTransform rectTransform => _rectTransform ??= this.transform as RectTransform;

        }
    }
}