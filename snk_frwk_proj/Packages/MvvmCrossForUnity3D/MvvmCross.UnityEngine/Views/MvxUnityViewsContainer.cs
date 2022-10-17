using System;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnityEngine.Views
{
    public class MvxUnityViewsContainer : MvxViewsContainer, IMvxUnityViewsContainer
    {
        public IMvxUnityView CreateView(MvxViewModelRequest request)
        {
            throw new NotImplementedException();
        }

        public IMvxUnityView CreateView(IMvxViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public IMvxUnityView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            throw new NotImplementedException();
        }
    }
}