using UnityEngine;

namespace SnkFramework.Mvvm.LayoutEngine
{
    namespace UGUI
    {
        public interface IUGUIView
        {
            public GameObject mGameObject { get; }
            public Transform mTransform { get; }
            public RectTransform mTectTransform { get; }
        }
    }
}