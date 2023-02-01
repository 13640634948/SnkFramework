using SnkFramework.Mvvm.Demo.Implements;
using SnkFramework.Mvvm.Demo.ViewModels;
using SnkFramework.Mvvm.Runtime;
using UnityEngine;

namespace SnkFramework.Mvvm.Demo
{
    public class DemoMvvmUI : MonoBehaviour
    {
        private ISnkMvvmService _mvvmService;
        private TestViewModel viewModel;

        private void Awake()
        {
            //_mvvmService = new DemoMvvmInstaller().Install();
        }

        async void Start()
        {
            viewModel = await _mvvmService.OpenWindow<TestViewModel>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _mvvmService.CloseWindow(viewModel);
        }
    }
}