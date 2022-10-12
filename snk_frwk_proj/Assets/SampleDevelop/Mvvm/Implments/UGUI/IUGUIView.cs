using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public interface IUGUIView
    {
        public GameObject mGameObject { get; }
        public Transform mTransform { get; }
        public RectTransform mTectTransform { get; }
    }
}