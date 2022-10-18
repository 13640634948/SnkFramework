using System;
using System.Threading.Tasks;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Unity.Presenters.Attributes;
using MvvmCross.Unity.Views;
using MvvmCross.ViewModels;
using UnityEngine;

namespace MvvmCross.Unity.Presenters
{
    public class MvxUnityViewPresenter : MvxAttributeViewPresenter, IMvxUnityViewPresenter
    {
        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxBasePresentationAttribute attr = null;
            if (viewType.IsSubclassOf(typeof(MvxUnityView)))
            {
                attr = new MvxUnityViewPresentationAttribute();
            }

            if (viewType.IsSubclassOf(typeof(MvxUnityWindow)))
            {
                attr = new MvxUnityWindowPresentationAttribute();
            }

            if (attr == null)
                throw new InvalidOperationException(
                    $"Don't know how to create a presentation attribute for type {viewType}");

            attr.ViewType = viewType;
            attr.ViewModelType = viewModelType;
            return attr;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxUnityViewPresentationAttribute>(ShowView, CloseView);
            AttributeTypesToActionsDictionary.Register<MvxUnityWindowPresentationAttribute>(ShowWindow, CloseWindow);
        }

        protected virtual Task<bool> ShowView(Type viewType,
            MvxUnityViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewType, attribute, request);

            /*
            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
            */
            var viewObject = Activator.CreateInstance(viewType);

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseView(IMvxViewModel viewModel, MvxUnityViewPresentationAttribute? attribute)
        {
            /*
            var currentView = CurrentActivity as IMvxView;

            if (currentView == null)
            {
                _logger.Value?.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe has no current page");
                return Task.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                _logger.Value?.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return Task.FromResult(false);
            }

            // don't kill the dead
            if (CurrentActivity.IsActivityAlive())
                CurrentActivity!.Finish();
*/
            return Task.FromResult(true);
        }


        protected virtual Task<bool> ShowWindow(Type windowType,
            MvxUnityWindowPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            Debug.Log("windowType:" + windowType);
            var viewObject = Activator.CreateInstance(windowType);
            return Task.FromResult(true);
        }


        protected virtual Task<bool> CloseWindow(IMvxViewModel viewModel,
            MvxUnityWindowPresentationAttribute? attribute)
            => Task.FromResult(true);

        private static void ValidateArguments(Type? view, MvxBasePresentationAttribute? attribute,
            MvxViewModelRequest? request)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            ValidateArguments(attribute, request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            ValidateArguments(attribute);

            ValidateArguments(request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

        private static void ValidateArguments(MvxViewModelRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }
    }
}