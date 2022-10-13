using SnkFramework.Mvvm.Core.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace UGUI
    {
        public interface IUGUIViewOwner : ISnkViewOwner
        {
            public CanvasGroup mCanvasGroup { get; }
            public Canvas mCanvas { get; }
        }
    }
}