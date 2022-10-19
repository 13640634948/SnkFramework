using MvvmCross.Unity.Views;

namespace MvvmCross.Unity.Presenters
{
    public interface IMvxUnityLayerContainer
    {
        public IMvxUnityLayer GetUnityLayer(string layerName);
        public TLayer GetUnityLayer<TLayer>() where TLayer : class, IMvxUnityLayer;
        public bool TryGetUnityLayer(string layerName, out IMvxUnityLayer unityLayer);
        public bool TryGetUnityLayer<TLayer>(string layerName, out TLayer unityLayer) where TLayer : class, IMvxUnityLayer;
    }
}