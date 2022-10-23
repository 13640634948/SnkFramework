using System;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UnityEngine;

namespace MvvmCross.Unity.Views
{
    public class MvxUnityViewsContainer : MvxViewsContainer, IMvxUnityViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }
        private IMvxViewModelLoader _viewModelLoader;
        protected IMvxViewModelLoader viewModelLoader => _viewModelLoader ??= Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();

        public IMvxUnityView CreateView(MvxViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = CreateViewOfType(viewType, request);
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    view.ViewModel = instanceRequest.ViewModelInstance;
                else
                    view.ViewModel = viewModelLoader.LoadViewModel(request, null);
                view.Created(null);
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }
        
        public virtual IMvxUnityView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            GameObject gameObject = new GameObject(viewType.Name);
            IMvxUnityView view = gameObject.AddComponent(viewType) as IMvxUnityView;
            //var view = Activator.CreateInstance(viewType) as IMvxUnityView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }
    }
}