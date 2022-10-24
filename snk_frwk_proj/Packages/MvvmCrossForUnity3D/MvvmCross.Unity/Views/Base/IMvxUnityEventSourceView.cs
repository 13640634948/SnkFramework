using System;
using MvvmCross.Base;

namespace MvvmCross.Unity.Views.Base
{
    public interface IMvxUnityEventSourceView : IMvxDisposeSource
    {
        public event EventHandler<MvxValueEventArgs<MvxUnityBundle>>? CreateCalled;
        //public event EventHandler? AppearingCalled;
        //public event EventHandler? AppearedCalled;
        public event EventHandler? LoadedCalled;
        public event EventHandler<MvxValueEventArgs<bool>>? ActivateCalled;
        public event EventHandler<MvxValueEventArgs<bool>>? PassivateCalled;

        public event EventHandler? DisappearingCalled;
        public event EventHandler? DisappearedCalled;
        public event EventHandler? DismissCalled;
    }
}