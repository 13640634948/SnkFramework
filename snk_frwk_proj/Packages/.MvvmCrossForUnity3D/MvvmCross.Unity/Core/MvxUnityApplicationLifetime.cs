using System;
using MvvmCross.Core;
using UnityEngine;

namespace MvvmCross.Unity.Core
{
    public class MvxUnityApplicationLifetime : MonoBehaviour,  IMvxUnityApplicationLifetime
    {
        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected void fireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        protected void OnApplicationQuit()
        {
            fireLifetimeChanged(MvxLifetimeEvent.Closing);
        }
    }
}