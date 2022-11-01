using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Layer
    {
        public interface ISnkLayer : ISnkBehaviourOwner
        {
            public string LayerName { get; }
            public void AddChild(SnkUIBehaviour windowUIBehaviour);
        }

        public interface ISnkLayerContainer
        {
            public void RegiestLayer(System.Type layerType);
            public void RegiestLayer<TLayer>() where TLayer : SnkUILayer;
            public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer;
            public void Build();
        }

        public class SnkLayerContainer : UIBehaviour, ISnkLayerContainer
        {
            private List<Type> _layerTypeList = new List<Type>();
            private Dictionary<System.Type, ISnkLayer> _layerDict = new Dictionary<Type, ISnkLayer>();

            public void RegiestLayer(Type layerType) => _layerTypeList.Add(layerType);

            public void RegiestLayer<TLayer>() where TLayer : SnkUILayer
                => _layerTypeList.Add(typeof(TLayer));

            public TLayer GetLayer<TLayer>() where TLayer : SnkUILayer
            {
                _layerDict.TryGetValue(typeof(TLayer), out var layer);
                return layer as TLayer;
            }

            public void Build()
            {
                foreach (var type in _layerTypeList)
                {
                    GameObject layerGameObject = new GameObject();
                    SnkUILayer layer = layerGameObject.AddComponent(type) as SnkUILayer;
                    _layerDict.Add(type, layer);
                    layerGameObject.name = layer.LayerName;
                    layerGameObject.transform.SetParent(transform);
                    layer.SetRectTransformIdentity();
                }
            }
        }

    }
}