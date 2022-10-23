using System.Collections.Generic;
using UnityEngine;

namespace MvvmCross.Unity.Views.UGUI
{
    public class MvxUGUILayerContainer : MonoBehaviour, IMvxUnityLayerContainer
    {
        private Dictionary<string, IMvxUnityLayer> _dictionary = new();

        public IMvxUnityLayer GetUnityLayer(string layerName)
        {
            if (_dictionary.TryGetValue(layerName, out IMvxUnityLayer unityLayer) == false)
                return null;
            return unityLayer;
        }

        public bool TryGetUnityLayer(string layerName, out IMvxUnityLayer unityLayer)
        {
            return _dictionary.TryGetValue(layerName, out unityLayer);
        }

        public void AddUnityLayer(IMvxUnityLayer unityLayer)
        {
            _dictionary.Add("normal", unityLayer);
        }
    }
}