using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUnityLayerBuilder
    {
        public void RegiestUnityLayerBuilder<TUnityLayer>()
            where TUnityLayer : class, IMvxUnityLayer;
        public IDictionary<string, IMvxUnityLayer> Build();
    }

    public class MvxUnityLayerBuilder : IMvxUnityLayerBuilder
    {
        private Dictionary<string, Type> _unityLayerTypeDict = new();

        public void RegiestUnityLayerBuilder<TUnityLayer>()
            where TUnityLayer : class, IMvxUnityLayer
        {
            System.Type layerType = typeof(TUnityLayer);
            _unityLayerTypeDict.Add(layerType.Name, layerType);
        }

        public IDictionary<string, IMvxUnityLayer> Build()
        {
            IViewCamera viewCamera = Mvx.IoCProvider.Resolve<IViewCamera>();
            
            IDictionary<string, IMvxUnityLayer> dict = new Dictionary<string, IMvxUnityLayer>();
            foreach (var kvp in _unityLayerTypeDict)
            {
                GameObject layerGameObject = new GameObject(kvp.Key);
                CanvasScaler scaler = layerGameObject.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                var layer = layerGameObject.AddComponent(kvp.Value) as IMvxUnityLayer;
                if (layer is MvxUGUILayer uguiLayer)
                {
                    uguiLayer.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    uguiLayer.Canvas.worldCamera = viewCamera.Current;
                }

                dict.Add(kvp.Key, layer);
            }
            return dict;
        }
    }

    public class MvxUnityLayerContainer : MonoBehaviour, IMvxUnityLayerContainer
    {
        private Dictionary<string, IMvxUnityLayer> _dictionary = new();

        public void AddAll(IDictionary<string, IMvxUnityLayer> unityLayerLookup)
        {
            foreach (var kvp in unityLayerLookup)
                AddUnityLayer(kvp.Key, kvp.Value);
        }

        public IMvxUnityLayer GetUnityLayer(string layerName)
        {
            if (_dictionary.TryGetValue(layerName, out IMvxUnityLayer unityLayer) == false)
                return null;
            return unityLayer;
        }

        public TUnityLayer GetUnityLayer<TUnityLayer>() where TUnityLayer : class, IMvxUnityLayer
        {
            IMvxUnityLayer layer = GetUnityLayer(typeof(TUnityLayer).Name);
            return layer as TUnityLayer;
        }

        public bool TryGetUnityLayer(string layerName, out IMvxUnityLayer unityLayer)
        {
            return _dictionary.TryGetValue(layerName, out unityLayer);
        }

        public void AddUnityLayer(string layerName, IMvxUnityLayer unityLayer)
        {
            _dictionary.Add(layerName, unityLayer);

            if (unityLayer is Component component)
                component.transform.SetParent(this.transform);
        }
    }
}