using SnkFramework.Mvvm.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.View
{
    public interface IUGUILayer : IUILayer
    {
        public Canvas mCanvas { get; }
        public CanvasScaler mCanvasScaler { get; }
    }
}