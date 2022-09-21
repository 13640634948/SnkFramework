using System.Collections;
using System.Collections.Generic;
using Windows.LoginWindow;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class WindowDemo : MonoBehaviour, IMvvmCoroutineExecutor
{
    public void RunOnCoroutineNoReturn(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    
    private List<LoginWindow> loginWindowList = new List<LoginWindow>();
    private IResourceUILocator locator;
    
    private void Awake()
    {
        UGUIWindowManager uguiWindowMgr = new UGUIWindowManager( );
        SnkMvvmSetup.Initialize(uguiWindowMgr,this);
    }

    void Start()
    {
        locator = new ResourceUILocator();
    }

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
                this.loginWindowList[0].Dismiss(true);
                this.loginWindowList.RemoveAt(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            var loginWindow = locator.LoadWindow<LoginWindow>(null);
            loginWindow.Show();
            this.loginWindowList.Add(loginWindow);
        }
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