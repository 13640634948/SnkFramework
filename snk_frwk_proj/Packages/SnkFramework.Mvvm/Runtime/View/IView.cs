using System;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.View
{
    public interface IView
    {
        public GameObject mOwner { get; }
        public IViewModel mViewModel { get; }
        public string mName { get; set; }
        public bool mVisibility { get; set; }

        public float Alpha { get; set; }

        public bool mInteractable { get; set; }
        public IAnimation mEnterAnimation { get; set; }
        public IAnimation mExitAnimation { get; set; }
        public UIAttribute[] mUIAttributes { get; }

        public CanvasGroup mCanvasGroup { get; }

        /// <summary>
        /// Triggered when the Visibility's value to be changed.
        /// </summary>
        event EventHandler VisibilityChanged;
        public void SetOwner(GameObject owner);
    }

    public interface IView<TViewModel> : IView
        where TViewModel : class, IViewModel
    {
        public new TViewModel mViewModel { get; }
    }
}