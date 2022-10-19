using System;
using System.Collections;
using MvvmCross.Base;
using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityWindow : IMvxUnityView
    {
        public event EventHandler? ShowingCalled;
        public event EventHandler<MvxValueEventArgs<bool>>? ShowedCalled;
        public event EventHandler? HidingCalled; 
        public event EventHandler<MvxValueEventArgs<bool>>? HiddenCalled;

        
        public IEnumerator Show(bool animated);
        public IEnumerator Hide(bool animated);

        public IMvxUnityLayer Layer { get; set; }
    }
    
    public interface IMvxUnityWindow<TViewModel, TUnityLayer, TUnityComponent> : IMvxUnityWindow, IMvxUnityView<TViewModel, TUnityComponent>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityLayer : class, IMvxUnityLayer
        where TUnityComponent : UnityEngine.Component
    {
    }
}