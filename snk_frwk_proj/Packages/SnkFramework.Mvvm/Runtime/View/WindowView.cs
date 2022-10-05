using System.Collections.Generic;
using SnkFramework.Mvvm.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public abstract class WindowView : View, IWindowView
    {
        public IAnimation mActivationAnimation { get; set; }
        public IAnimation mPassivationAnimation { get; set; }

        private List<IView> _viewList;
        public List<IView> Views => _viewList;

        public IView GetView(string name)
        {
            int index = this._viewList.FindIndex(a => a.mName.Equals(name));
            return index >= 0 ? this._viewList[index] : null;
        }

        public void AddView(IView view, bool worldPositionStays = false)
        {
            if (viewValidityCheck(view) == false)
                return;

            Transform t = view.mOwner.transform;
            if (t.parent == this.mOwner.transform)
                return;

            view.mOwner.gameObject.layer = this.mOwner.gameObject.layer;
            t.SetParent(this.mOwner.transform, worldPositionStays);
            this._viewList.Add(view);
        }

        public void AddView(IView view, UILayoutOverride layoutOverride)
        {
            if (viewValidityCheck(view) == false)
                return;

            Transform t = view.mOwner.transform;
            if (t.parent != this.mOwner.transform)
            {
                view.mOwner.gameObject.layer = this.mOwner.gameObject.layer;
                t.SetParent(this.mOwner.transform, false);
                this._viewList.Add(view);
            }

            layoutOverride?.Invoke(view.mOwner.transform as RectTransform);
        }

        public void RemoveView(IView view, bool worldPositionStays = false)
        {
            if (viewValidityCheck(view) == false)
                return;
            
            Transform t = view.mOwner.transform;
            if (t == null || t.parent != this.mOwner.transform)
                return;

            t.SetParent(null, worldPositionStays);

            this._viewList.Remove(view);
        }
    }
}