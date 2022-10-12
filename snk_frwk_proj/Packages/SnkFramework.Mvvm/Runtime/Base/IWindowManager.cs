using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base

{
    public interface IWindowManager
    {
        public ISnkUILayer GetLayer(string layerName);    

    }
}