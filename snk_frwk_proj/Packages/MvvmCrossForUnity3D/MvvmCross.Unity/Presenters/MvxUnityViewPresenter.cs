using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MvvmCross.Core;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Unity.Core;
using MvvmCross.Unity.Presenters.Attributes;
using MvvmCross.Unity.Views;
using MvvmCross.Unity.Views.UGUI;
using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Presenters
{
    public partial class MvxUnityViewPresenter : MvxAttributeViewPresenter, IMvxUnityViewPresenter
    {
        private IMvxUnityViewCreator _viewCreator;
        protected IMvxUnityViewCreator viewCreator => _viewCreator ??= Mvx.IoCProvider.Resolve<IMvxUnityViewCreator>();

        private IMvxUnityLayerContainer _layerContainer;
        protected IMvxUnityLayerContainer layerContainer => _layerContainer ??= Mvx.IoCProvider.Resolve<IMvxUnityLayerContainer>();
        private List<CancellationTokenSource> cancellationTokenSourceList = new ();

        public MvxUnityViewPresenter()
        {
            var applicationLifetime = Mvx.IoCProvider.Resolve<IMvxUnityApplicationLifetime>();
            applicationLifetime.LifetimeChanged += (sender, args) =>
            {
                if (args.LifetimeEvent == MvxLifetimeEvent.Closing)
                {
                    foreach (var cancellationTokenSource in cancellationTokenSourceList)
                    {
                        cancellationTokenSource.Cancel();
                    }
                    cancellationTokenSourceList.Clear();
                }
            };
        }

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
            ValidateArguments(windowType, attribute, request);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSourceList.Add(cancellationTokenSource);
            IMvxUnityWindow window = await viewCreator.CreateView(request) as IMvxUnityWindow;
            if (window == null)
                throw new NullReferenceException("window is null");
            window.Created(null);
            window.OnLoaded();
            var layer = layerContainer.GetUnityLayer<MvxUGUINormalLayer>();
            layer.Add(window);
            bool cancel = await layer.ShowTransition(window, true).WithCancellation(cancellationTokenSource.Token).SuppressCancellationThrow();
            if (cancel)
            {
                //todo:cancel
            }
            cancellationTokenSourceList.Remove(cancellationTokenSource);
            return true;
        }

        protected virtual Task<bool> CloseWindow(IMvxViewModel viewModel, MvxUnityWindowAttribute? attribute)
            => Task.FromResult(true);

    }
}