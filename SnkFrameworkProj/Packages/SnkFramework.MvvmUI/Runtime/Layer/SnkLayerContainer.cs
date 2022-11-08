using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        [RequireComponent(typeof(RectTransform))]
        public class SnkLayerContainer : UIBehaviour, ISnkLayerContainer
        {
            private List<Type> _layerTypeList = new List<Type>();
            private Dictionary<System.Type, ISnkLayer> _layerDict = new Dictionary<Type, ISnkLayer>();

            public void RegiestLayer(Type layerType) => _layerTypeList.Add(layerType);

            public void RegiestLayer<TLayer>() where TLayer : SnkUILayer
                => _layerTypeList.Add(typeof(TLayer));

            public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer
                => GetLayer(typeof(TLayer)) as TLayer;

            public ISnkLayer GetLayer(Type layerType)
            {
                _layerDict.TryGetValue(layerType, out var layer);
                return layer;
            }

            public void Build(ISnkViewCamera viewCamera)
            {
                foreach (var type in _layerTypeList)
                {
                    GameObject layerGameObject = new GameObject();
                    SnkUILayer layer = layerGameObject.AddComponent(type) as SnkUILayer;
                    layer.canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    layer.canvas.worldCamera = viewCamera.ViewCamera;
                    _layerDict.Add(type, layer);
                    layerGameObject.name = layer.LayerName;
                    layerGameObject.transform.SetParent(transform);
                    layer.SetRectTransformIdentity();
                }
            }
        }
    }
}