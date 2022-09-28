using SnkFramework.Mvvm.Base;

public abstract class WindowManagerBase : IWindowManager
{
    public abstract IUILayer GetLayer(string layerName);
}