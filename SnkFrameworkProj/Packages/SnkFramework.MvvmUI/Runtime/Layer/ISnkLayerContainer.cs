namespace SnkFramework.Mvvm.Runtime.Layer
{
    public interface ISnkLayerContainer
    {
        public void RegiestLayer(System.Type layerType);
        public void RegiestLayer<TLayer>() where TLayer : SnkUILayer;
        public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer;
        public ISnkLayer GetLayer(System.Type layerType);
        public void Build(ISnkViewCamera viewCamera);
    }
}