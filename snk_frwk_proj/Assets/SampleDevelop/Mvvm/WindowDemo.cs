using System;
using System.Collections;
using System.Collections.Generic;
using Windows.LoginWindow;
using Loxodon.Framework.Interactivity;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;

public class MvvmLoader : IMvvmLoader
{
    public ISnkViewOwner LoadViewOwner(string ownerPath)
    {
        Debug.Log(ownerPath);
        GameObject asset = Resources.Load<GameObject>(ownerPath);
        GameObject inst = GameObject.Instantiate(asset);
        return inst.AddComponent<UGUIViewOwner>();
    }

    public IEnumerator LoadViewOwnerAsync(string ownerPath, Action<ISnkViewOwner> callback)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(ownerPath);
        yield return request;
        GameObject inst = GameObject.Instantiate(request.asset as GameObject);
        callback.Invoke(inst.AddComponent<UGUIViewOwner>());
    }
}

public class WindowDemo : MonoBehaviour, IMvvmCoroutineExecutor
{
    public void RunOnCoroutineNoReturn(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    private List<LoginWindow> loginWindowList = new List<LoginWindow>();

    private UGUIWindowManager _uguiWindowMgr;
    private void Awake()
    {
        _uguiWindowMgr= new UGUIWindowManager();
        SnkMvvmSetup.Initialize(_uguiWindowMgr, this, new MvvmLoader());
    }

    void Start()
    {
    }

    public bool mIgnoreAnimation;
    void Update()
    {
        foreach (var window in loginWindowList)
        {
            if (window != null)
                window.mViewModel.Tip = Time.frameCount.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (this.loginWindowList.Count > 0)
            {
                int index = this.loginWindowList.Count-1;
                this.loginWindowList[index].Dismiss(mIgnoreAnimation);
                this.loginWindowList.RemoveAt(index);
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (this.loginWindowList.Count > 0)
            {
                int index = this.loginWindowList.Count-1;
                (this.loginWindowList[index].mViewModel.mInteractionFinished as InteractionRequest).Raise();
            } 
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            RunOnCoroutineNoReturn(LoadWindowAsync());
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            //SnkMvvmSetup.mMvvmLog.InfoFormat("[{0}]KeyCode.S-0", Time.frameCount);
            //var window = locator.LoadWindow<LoginWindow>(null);
            var window = _uguiWindowMgr.OpenWindow<LoginWindow>();
            //AlphaAnimation alphaAnimation = window.mOwner.GetComponent<AlphaAnimation>();
            //alphaAnimation.Init(window);

            /*
            IAnimation[] anims = 
            {
                new AlphaAnimation{
                    AnimType = ANIM_TYPE.enter_anim,
                    from = 0.0f,
                    to = 1.0f,
                    duration = 1.0f
                },
                new AlphaAnimation{
                    AnimType = ANIM_TYPE.exit_anim,
                    from = 1.0f,
                    to = 0.0f,
                    duration = 1.0f
                }
            };
            
            foreach (var anim in anims)
                anim.Initialize(window);
            */
            
            window.Show(mIgnoreAnimation);
            //SnkMvvmSetup.mMvvmLog.InfoFormat("[{0}]KeyCode.S-1", Time.frameCount);
            this.loginWindowList.Add(window);
        }
    }

    private IEnumerator LoadWindowAsync()
    {
        yield return _uguiWindowMgr.OpenWindowAsync<LoginWindow>( window =>
        {
            window.Show(mIgnoreAnimation);
            this.loginWindowList.Add(window);
        });
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        foreach (var window in loginWindowList)
        {
            window.Dismiss(true);
        }

        this.loginWindowList.Clear();
        this.loginWindowList = null;
    }
}