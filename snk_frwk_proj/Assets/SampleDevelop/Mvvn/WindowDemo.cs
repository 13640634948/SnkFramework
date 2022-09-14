using Windows.LoginWindow;
using SnkFramework.FluentBinding.Base;
using UnityEngine;

public class WindowDemo : MonoBehaviour
{
    private void Awake()
    {
        SnkBindingSetup.Initialize();
    }

    private LoginWindow loginWindow;
    void Start()
    {
        IResourceUILocator locator = new ResourceUILocator();
        loginWindow = locator.LoadWindow<LoginWindow>(null);
    }

    // Update is called once per frame
    void Update()
    {
        loginWindow.mViewModel.Tip = Time.frameCount.ToString();
    }
}