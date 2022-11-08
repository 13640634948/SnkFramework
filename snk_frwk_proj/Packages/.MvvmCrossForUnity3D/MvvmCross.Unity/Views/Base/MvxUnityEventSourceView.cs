using System;
using MvvmCross.Base;

namespace MvvmCross.Unity.Views.Base
{
    public class MvxUnityEventSourceView : IMvxUnityEventSourceView
    {
        public event EventHandler<MvxValueEventArgs<MvxUnityBundle>> CreateCalled
        {
            add => createCalled += value;
            remove => createCalled -= value;
        }

        public event EventHandler? LoadedCalled  
        {
            add => loadedCalled += value;
            remove => loadedCalled -= value;
        }

        /*
        public event EventHandler AppearingCalled
        {
            add => appearingCalled += value;
            remove => appearingCalled -= value;
        }
        public event EventHandler AppearedCalled
        {
            add => appearedCalled += value;
            remove => appearedCalled -= value;
        }
        */
        
        public event EventHandler<MvxValueEventArgs<bool>> ActivateCalled
        {
            add => activateCalled += value;
            remove => activateCalled -= value;
        }
        public event EventHandler<MvxValueEventArgs<bool>> PassivateCalled
        {
            add => passivateCalled += value;
            remove => passivateCalled -= value;
        }
        public event EventHandler DisappearingCalled
        {
            add => disappearingCalled += value;
            remove => disappearingCalled -= value;
        }
        public event EventHandler DisappearedCalled
        {
            add => disappearedCalled += value;
            remove => disappearedCalled -= value;
        }
        public event EventHandler DismissCalled
        {
            add => dismissCalled += value;
            remove => dismissCalled -= value;
        }
        public event EventHandler? DisposeCalled
        {
            add => disposeCalled += value;
            remove => disposeCalled -= value;
        }

        protected EventHandler<MvxValueEventArgs<MvxUnityBundle>>? createCalled;
        //protected EventHandler? appearingCalled;
        //protected EventHandler? appearedCalled;
        protected EventHandler? loadedCalled;
        protected EventHandler<MvxValueEventArgs<bool>>? activateCalled;
        protected EventHandler<MvxValueEventArgs<bool>>? passivateCalled;
        protected EventHandler? disappearingCalled;
        protected EventHandler? disappearedCalled;
        protected EventHandler? dismissCalled;
        protected EventHandler? disposeCalled;

    }
}