using MvvmCross.Unity.ViewModels;
using UnityEngine;

namespace MvvmCross.Demo
{
    public class DemoViewModel : MvxUnityViewModel<DemoStartupViewModelParameter>
    {
        public DemoViewModel()
        {
            UnityEngine.Debug.Log("DemoViewModel.ctor");
        }

        public override void Prepare()
        {
            base.Prepare();
            Debug.Log("Prepare.parameter");
        }

        public override void Prepare(DemoStartupViewModelParameter parameter)
        {
            base.Prepare(parameter);
            Debug.Log("Prepare.parameter:" + parameter.mIndex);
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            UnityEngine.Debug.Log("DemoViewModel.ViewCreated");
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            UnityEngine.Debug.Log("DemoViewModel.ViewAppearing");
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            UnityEngine.Debug.Log("DemoViewModel.ViewAppeared");
        }
    }
}