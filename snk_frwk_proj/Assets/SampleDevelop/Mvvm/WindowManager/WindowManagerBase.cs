using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;

public abstract class WindowManagerBase : IWindowManager
{
    public abstract ISnkUILayer GetLayer(string layerName);
}