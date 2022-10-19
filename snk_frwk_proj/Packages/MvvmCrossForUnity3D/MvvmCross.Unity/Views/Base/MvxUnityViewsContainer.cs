using System;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Unity.Views
{
    public class MvxUnityViewsContainer : MvxViewsContainer, IMvxUnityViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public IMvxUnityView CreateView(MvxViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = CreateViewOfType(viewType, request);
                //view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }
        
        public virtual IMvxUnityView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {

            var view = Activator.CreateInstance(viewType) as IMvxUnityView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }
    }
}