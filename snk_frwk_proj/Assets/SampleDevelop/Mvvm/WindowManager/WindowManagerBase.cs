using SampleDevelop.Test;
using SnkFramework.Mvvm.Base;

public abstract class WindowManagerBase : IWindowManager
{
    public abstract ISnkUILayer GetLayer(string layerName);
}