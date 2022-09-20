using System;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public interface IUILayerFactory
    {
        public ISnkUIMainLayer CreateMainLayer();
        public IUILayer CreateUILayer();
    }

    internal class UILayerFactory : IUILayerFactory
    {
        public ISnkUIMainLayer CreateMainLayer()
        {
            throw new NotImplementedException();
        }

        public IUILayer CreateUILayer()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICameraRoot
    {
        public Camera mMainCamera { get; }
        public Camera mUICamera { get; }
    }

    internal class SnkCameraRoot : ICameraRoot
    {
        public Camera mMainCamera { get; }
        public Camera mUICamera { get; }

        public SnkCameraRoot()
        {
            GameObject asset = Resources.Load<GameObject>("CameraRoot");
            GameObject inst = GameObject.Instantiate(asset);
            GameObject.DontDestroyOnLoad(inst);
            mMainCamera = inst.transform.Find("Main Camera").GetComponent<Camera>();
            mUICamera = inst.transform.Find("UICamera").GetComponent<Camera>();
        }
    }

    public class SnkMvvmSetup
    {
        static public ISnkMvvmSettings mSettings;
        static public ISnkUIMainLayer mMainLayer;
        static public IWindowManager mWindowManager;
        static public ICameraRoot mCameraRoot;
        static public IMvvmLog mMvvmLog;

        static public void Initialize(
            ISnkUIMainLayer mainLayer = null, 
            ICameraRoot cameraRoot = null,
            ISnkMvvmSettings settings = null,
            IMvvmLog mvvmLog = null)
        {
            mSettings = settings ??= new SnkMvvmSettings();
            mCameraRoot = cameraRoot ??= new SnkCameraRoot();
            mMvvmLog = mvvmLog ??= new SnkMvvmLog();
            mMainLayer = mainLayer ??= createUIMainLayer();
        }

        static public ISnkUIMainLayer createUIMainLayer()
        {
            GameObject asset = Resources.Load<GameObject>("WindowRoot");
            GameObject inst = GameObject.Instantiate(asset);
            GameObject.DontDestroyOnLoad(inst);
            SnkUIMainLayer mainLayer = inst.AddComponent<SnkUIMainLayer>();
            Canvas canvas = mainLayer.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = mCameraRoot.mUICamera;
            return mainLayer;
        }
    }
}