using System;
using System.Threading.Tasks;
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
        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxBasePresentationAttribute attr = null;
            /*
            if (viewType.IsSubclassOf(typeof(IMvxUnityPopupWindow)))
            {
                attr = new MvxUnityPopupWindowAttribute();
            }

            if (viewType.IsSubclassOf(typeof(IMvxUnityFullWindow)))
            {
                attr = new MvxUnityFullWindowAttribute();
            }
            */

            if (typeof(IMvxUnityPopupWindow).IsAssignableFrom(viewType))
            {
                attr = new MvxUnityPopupWindowAttribute();
            }

            if (typeof(IMvxUnityFullWindow).IsAssignableFrom(viewType))
            {
                attr = new MvxUnityPopupWindowAttribute();
            }

            if (attr == null)
                throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {viewType}");

            attr.ViewType = viewType;
            attr.ViewModelType = viewModelType;
            return attr;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxUnityPopupWindowAttribute>(ShowPopupWindow,
                ClosePopupWindow);
            AttributeTypesToActionsDictionary.Register<MvxUnityFullWindowAttribute>(ShowFullWindow,
                CloseFullWindow);
        }

        
        protected virtual Task<bool> ShowPopupWindow(Type viewType,
            MvxUnityPopupWindowAttribute attribute,
            MvxViewModelRequest request)
        {
             IMvxUnityView popupWindow = viewCreator.CreateView(request);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ClosePopupWindow(IMvxViewModel viewModel,
            MvxUnityPopupWindowAttribute? attribute)
            => Task.FromResult(true);

        protected virtual Task<bool> ShowFullWindow(Type windowType,
            MvxUnityFullWindowAttribute attribute,
            MvxViewModelRequest request)
        {
            IMvxUnityView fullWindow = viewCreator.CreateView(request);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseFullWindow(IMvxViewModel viewModel,
            MvxUnityFullWindowAttribute? attribute)
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