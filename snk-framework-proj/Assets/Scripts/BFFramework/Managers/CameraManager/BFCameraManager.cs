using UnityEngine;

namespace BFFramework.Runtime.Managers
{
    public class BFCameraManager : BFManager<BFCameraManager>, IBFCameraManager
    {
        public Camera mainCamera { get; protected set; }
        
        public Camera perspectiveCamera { get; protected set;}
        public Camera orthographicCamera { get; protected set;}

        public BFCameraManager()
        {
            this.InitializeMainCamera();
            this.InitializePerspectiveCamera();
            this.InitializeOrthographicCamera();
        }

        protected virtual void InitializeMainCamera()
        {
        }
        
        protected virtual void InitializePerspectiveCamera()
        {
        }

        protected virtual void InitializeOrthographicCamera()
        {
            var viewCameraGameObject = new GameObject(nameof(Camera));
            orthographicCamera = viewCameraGameObject.AddComponent<Camera>();
            orthographicCamera.clearFlags = CameraClearFlags.SolidColor;
            orthographicCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            GameObject.DontDestroyOnLoad(viewCameraGameObject); 
        }
    }
}