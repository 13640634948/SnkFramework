using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Unity.Presenters.Attributes;
using MvvmCross.Unity.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Presenters
{
    public class MvxUnityViewPresenter : MvxAttributeViewPresenter, IMvxUnityViewPresenter
    {
        private IMvxUnityViewCreator _viewCreator;
        protected IMvxUnityViewCreator viewCreator => _viewCreator ??= Mvx.IoCProvider.Resolve<IMvxUnityViewCreator>();

        private IMvxUnityLayerContainer _layerContainer;
        protected IMvxUnityLayerContainer layerContainer => _layerContainer ??= Mvx.IoCProvider.Resolve<IMvxUnityLayerContainer>();

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
            IMvxUnityWindow window = await viewCreator.CreateView(request) as IMvxUnityWindow;
            if (window == null)
                throw new NullReferenceException("window is null");
            window.Created(null);
            window.OnLoaded();
            var layer = layerContainer.GetUnityLayer("normal");
            layer.Add(window);
            await layer.ShowTransition(window, true);
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