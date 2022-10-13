using SnkFramework.Mvvm.Core.View;
using UnityEngine;
using UnityEngine.UI;


namespace SnkFramework.Mvvm.Runtime
{
    namespace UGUI
    {
        public interface IUGUILayer : ISnkUILayer
        {
            public Canvas mCanvas { get; }
            public CanvasScaler mCanvasScaler { get; }
        }
    }
}