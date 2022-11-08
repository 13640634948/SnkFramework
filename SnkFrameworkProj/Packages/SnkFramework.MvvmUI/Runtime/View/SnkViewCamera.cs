using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        [RequireComponent(typeof(Camera))]
        public class SnkViewCamera : MonoBehaviour, ISnkViewCamera
        {
            private Camera _viewCamera;
            public Camera ViewCamera => _viewCamera ??= this.GetComponent<Camera>();
        }
    }
}