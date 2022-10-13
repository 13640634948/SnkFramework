using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
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