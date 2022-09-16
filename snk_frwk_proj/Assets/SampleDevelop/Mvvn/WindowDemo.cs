using Windows.LoginWindow;
using SnkFramework.FluentBinding.Base;
using SnkFramework.Mvvm.Base;
using UnityEngine;

public class WindowDemo : MonoBehaviour
{
    private void Awake()
    {
        SnkBindingSetup.Initialize();
        SnkMvvmSetup.Initialize();
    }

    private LoginWindow loginWindow;
    private IResourceUILocator locator;

    void Start()
    {
        locator = new ResourceUILocator();
    }

    void Update()
    {
        if (loginWindow != null)
            loginWindow.mViewModel.Tip = Time.frameCount.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (loginWindow != null)
                loginWindow.Dismiss(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            loginWindow = locator.LoadWindow<LoginWindow>(null);
            loginWindow.Show();
        }
    }
}