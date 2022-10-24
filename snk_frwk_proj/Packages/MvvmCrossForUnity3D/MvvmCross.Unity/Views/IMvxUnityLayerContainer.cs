using System.Collections.Generic;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityLayerContainer
    {
        public void AddAll(IDictionary<string, IMvxUnityLayer> viewModelViewLookup);

        public IMvxUnityLayer GetUnityLayer(string layerName);
        public TUnityLayer GetUnityLayer<TUnityLayer>() where TUnityLayer : class, IMvxUnityLayer;
        
        public bool TryGetUnityLayer(string layerName, out IMvxUnityLayer unityLayer);
        public void AddUnityLayer(string layerName, IMvxUnityLayer unityLayer);

    }
}