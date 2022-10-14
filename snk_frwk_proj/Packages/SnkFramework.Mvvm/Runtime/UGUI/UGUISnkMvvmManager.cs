using SnkFramework.Mvvm.Core;
using SnkFramework.Mvvm.Core.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace UGUI
    {
        public class UGUISnkMvvmManager : SnkMvvmManagerBase<UGUILayer>
        {
            public Camera mViewCamera { get; protected set; }

            public GameObject mOwner;

            protected override void initialize()
            {
                base.initialize();
                GameObject asset = Resources.Load<GameObject>("WindowRoot");
                GameObject inst = GameObject.Instantiate(asset);
                GameObject.DontDestroyOnLoad(inst);
                mViewCamera = inst.transform.Find("ViewCamera").GetComponent<Camera>();
                this.mOwner = inst;

                int layerCount = (int) LAYER.COUNT;
                for (int i = 0; i < layerCount; i++)
                {
                    string layerName = System.Enum.GetName(typeof(LAYER), (LAYER) i);
                    this.CreateLayer(layerName);
                }
            }

            public override ISnkUILayer CreateLayer(string layerName)
            {
                if (this.TryGetLayer(layerName, out var layer))
                    return null;

                GameObject asset = Resources.Load<GameObject>("UILayer");
                GameObject inst = GameObject.Instantiate(asset);
                inst.name = layerName;
                if (inst.TryGetComponent<Canvas>(out var canvas) == false)
                    canvas = inst.AddComponent<Canvas>();
                inst.transform.SetParent(this.mOwner.transform);
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = this.mViewCamera;
                canvas.sortingLayerID = SortingLayer.NameToID(layerName);

                UGUILayer uguiLayer = new UGUILayer();
                uguiLayer.SetOwner(canvas);
                this.layerDict.Add(layerName, uguiLayer);
                return uguiLayer;
            }
        }
    }
}