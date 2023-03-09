using UnityEngine;

namespace BFFramework.Runtime.Managers
{
    public interface IBFCameraManager
    {
        /// <summary>
        /// 主摄像机
        /// </summary>
        public Camera mainCamera { get; }

        /// <summary>
        /// 透视摄像机
        /// </summary>
        public Camera perspectiveCamera { get; }
        
        /// <summary>
        /// 正交摄像机
        /// </summary>
        public Camera orthographicCamera { get; }
    }
}