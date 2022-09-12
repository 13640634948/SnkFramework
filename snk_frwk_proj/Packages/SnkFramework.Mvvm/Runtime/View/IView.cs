using SnkFramework.Mvvm.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public interface IView
    {
        public GameObject mOwner { get; }
        public string mName { get; }
        public bool mActivated { get; }

        public bool mInteractable { get; set; }
        public UIAnimation mEnterAnimation { get; set; }
        public UIAnimation mExitAnimation { get; set; }
        public UIAttribute[] mUIAttributes { get; set; }
            
        public void SetOwner(GameObject owner);
        public void InitComponents();
    }
}