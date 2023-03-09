using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Extensions;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.NuGet.Exceptions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        [RequireComponent(typeof(RectTransform), typeof(StandaloneInputModule))]
        public class SnkLayerContainer : UIBehaviour, ISnkLayerContainer
        {
            private List<Type> _layerTypeList = new List<Type>();
            private Dictionary<System.Type, ISnkLayer> _layerDict = new Dictionary<Type, ISnkLayer>();
            
            private System.Type _defaultLayerType;
            public Type DefaultLayerType => _defaultLayerType;
            
            public ISnkLayer GetDefaultLayer() => this.GetLayer(_defaultLayerType);

            public TLayer GetDefaultLayer<TLayer>() where TLayer : SnkUILayer
                => GetDefaultLayer() as TLayer;

            public void RegiestLayer(Type layerType, bool defaultLayer = false)
            {
                if (defaultLayer)
                {
                    if (_defaultLayerType != null)
                        new SnkException("只能有一个默认层级");
                    this._defaultLayerType = layerType;
                }
                _layerTypeList.Add(layerType);
            }

            public void RegiestLayer<TLayer>(bool defaultLayer = false) where TLayer : SnkUILayer
                => RegiestLayer(typeof(TLayer), defaultLayer);

            public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer
                => GetLayer(typeof(TLayer)) as TLayer;

            public ISnkLayer GetLayer(Type layerType)
            {
                _layerDict.TryGetValue(layerType, out var layer);
                return layer;
            }

            public void Build(Camera orthographicCamera)
            {
                foreach (var type in _layerTypeList)
                {
                    var layerGameObject = new GameObject();
                    var layer = layerGameObject.AddComponent(type) as SnkUILayer;
                    layer.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    layer.Canvas.worldCamera = orthographicCamera;
                    _layerDict.Add(type, layer);
                    layerGameObject.name = layer.LayerName;
                    layerGameObject.transform.SetParent(transform);
                    layer.rectTransform.Identity();
                }
            }
        }
    }
}