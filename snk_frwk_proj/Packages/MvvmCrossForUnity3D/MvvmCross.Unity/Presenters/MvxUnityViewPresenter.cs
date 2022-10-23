using System;
using System.Threading.Tasks;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Unity.Base.ResourceService;
using MvvmCross.Unity.Presenters.Attributes;
using MvvmCross.Unity.Views;
using MvvmCross.ViewModels;

using Cysharp.Threading.Tasks;
using MvvmCross.Unity.Views.UGUI;
using UnityEngine;

namespace MvvmCross.Unity.Presenters
{
    public class MvxUnityViewPresenter : MvxAttributeViewPresenter, IMvxUnityViewPresenter
    {
        private IMvxUnityViewCreator _viewCreator;
        protected IMvxUnityViewCreator viewCreator => _viewCreator ??= Mvx.IoCProvider.Resolve<IMvxUnityViewCreator>();

        private IMvxUnityResourceService _resourceService;

        protected IMvxUnityResourceService resourceServicer =>
            this._resourceService ??= Mvx.IoCProvider.Resolve<IMvxUnityResourceService>();

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxBasePresentationAttribute attr = new MvxUnityWindowAttribute();
            attr.ViewType = viewType;
            attr.ViewModelType = viewModelType;
            return attr;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxUnityWindowAttribute>(ShowWindow, CloseWindow);
        }

        async protected virtual Task<bool> ShowWindow(Type windowType, MvxUnityWindowAttribute attribute, MvxViewModelRequest request)
        {
            IMvxUnityView window = viewCreator.CreateView(request);
            window.Appearing();
            var asset = await resourceServicer.LoadBuildInResourceAsync<GameObject>("Prefab/" + windowType.Name);
            window.Appeared(GameObject.Instantiate(asset).AddComponent<MvxUGUIOwner>());
            return true;
        }

        protected virtual Task<bool> CloseWindow(IMvxViewModel viewModel, MvxUnityWindowAttribute? attribute)
            => Task.FromResult(true);

        protected void ValidateArguments(Type? view, MvxBasePresentationAttribute? attribute,
            MvxViewModelRequest? request)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            ValidateArguments(attribute, request);
        }

        protected void ValidateArguments(MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            ValidateArguments(attribute);

            ValidateArguments(request);
        }

        protected void ValidateArguments(MvxBasePresentationAttribute? attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

        protected void ValidateArguments(MvxViewModelRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }
    }
}