using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkLayerContainer
        {
            public System.Type DefaultLayerType { get; }
            public ISnkLayer GetDefaultLayer();
            public TLayer GetDefaultLayer<TLayer>() where TLayer : SnkUILayer;
            
            public void RegiestLayer(System.Type layerType, bool defaultLayer = false);
            public void RegiestLayer<TLayer>(bool defaultLayer = false) where TLayer : SnkUILayer;
            public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer;
            public ISnkLayer GetLayer(System.Type layerType);
            public void Build(Camera orthographicCamera);
        }
    }
}