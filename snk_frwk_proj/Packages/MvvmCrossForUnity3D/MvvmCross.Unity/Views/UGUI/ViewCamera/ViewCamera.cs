using UnityEngine;

namespace MvvmCross.Unity.Views.UGUI
{
    [RequireComponent(typeof(Camera))]
    public class ViewCamera : MonoBehaviour, IViewCamera
    {
        private Camera _camera;
        public Camera Current => _camera??= GetComponent<Camera>();
    }
}