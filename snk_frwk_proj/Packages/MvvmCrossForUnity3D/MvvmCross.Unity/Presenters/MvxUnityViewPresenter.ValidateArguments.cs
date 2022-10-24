using System;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Presenters
{
    public partial class MvxUnityViewPresenter
    {
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