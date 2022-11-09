using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        public interface ISnkUINodeUnit
        {
            public Canvas Canvas { get; }
            public CanvasGroup CanvasGroup { get; }
            
            /// <summary>
            /// 视图是否可交互
            /// </summary>
            public bool Interactable { get; set; }
        }
    }
}