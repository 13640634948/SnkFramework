using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public interface IUGUIViewOwner : ISnkViewOwner
    {
        public CanvasGroup mCanvasGroup { get; }
        public Canvas mCanvas { get; }
    }
}