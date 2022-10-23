using System;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Unity.Base.ResourceService;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UnityEngine;

namespace MvvmCross.Unity.Views
{
    public class MvxUnityViewsContainer : MvxViewsContainer, IMvxUnityViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }
        private IMvxViewModelLoader _viewModelLoader;

        protected IMvxViewModelLoader viewModelLoader =>
            _viewModelLoader ??= Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();

        private IMvxUnityResourceService _resourceService;

        protected IMvxUnityResourceService resourceServicer =>
            this._resourceService ??= Mvx.IoCProvider.Resolve<IMvxUnityResourceService>();

        async public Task<IMvxUnityView> CreateView(MvxViewModelRequest request)
        {
            IMvxUnityView view = default;
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                view = await CreateViewOfType(viewType, request);
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    view.ViewModel = instanceRequest.ViewModelInstance;
                else
                    view.ViewModel = viewModelLoader.LoadViewModel(request, null);
                view.Created(null);
            }
            finally
            {
                CurrentRequest = null;
            }
            return view;
        }

        async public virtual Task<IMvxUnityView> CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            var asset = await resourceServicer.LoadBuildInResourceAsync<GameObject>("Prefab/" + viewType.Name);
            GameObject inst = GameObject.Instantiate(asset);
            IMvxUnityView view = inst.AddComponent(viewType) as IMvxUnityView;
            //var view = Activator.CreateInstance(viewType) as IMvxUnityView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }
    }
}