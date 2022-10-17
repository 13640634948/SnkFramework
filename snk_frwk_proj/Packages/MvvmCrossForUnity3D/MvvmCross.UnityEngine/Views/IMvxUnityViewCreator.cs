using System;
using MvvmCross.ViewModels;

namespace MvvmCross.UnityEngine.Views
{
    public interface IMvxUnityViewCreator
    {
        IMvxUnityView CreateView(MvxViewModelRequest request);

        IMvxUnityView CreateView(IMvxViewModel viewModel);

        IMvxUnityView CreateViewOfType(Type viewType, MvxViewModelRequest request);
    }
}