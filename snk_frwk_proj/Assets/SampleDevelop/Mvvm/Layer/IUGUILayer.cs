using SampleDevelop.Test;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.View
{
    public interface IUGUILayer : ISnkUILayer
    {
        public Canvas mCanvas { get; }
        public CanvasScaler mCanvasScaler { get; }
    }
}