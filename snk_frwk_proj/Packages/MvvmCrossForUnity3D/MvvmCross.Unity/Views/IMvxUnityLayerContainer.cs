namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityLayerContainer
    {
        public IMvxUnityLayer GetUnityLayer(string layerName);
        public bool TryGetUnityLayer(string layerName, out IMvxUnityLayer unityLayer);

        public void AddUnityLayer(IMvxUnityLayer unityLayer);
    }
}