using System.Collections;
using Windows.LoginWindow;
using SnkFramework.Mvvm.View;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public abstract class UGUIWindow<TViewModel> : SnkWindow<UGUIViewOwner, UGUILayer, TViewModel>
        where TViewModel : class, ISnkViewModel, new()
    {
        public override string mName
        {
            get => this.mOwner.name;
            set => base.mName = value;
        }
        private GameObject _gameObject;
        protected GameObject gameObject => _gameObject ??= this.mOwner.gameObject;

        private Transform _transform;
        protected Transform transform => _transform ??= this.mOwner.transform;

        private RectTransform _rectTransform;
        protected RectTransform rectTransform => _rectTransform ??= transform as RectTransform;

        public override void LoadViewOwner()
        {
            base.LoadViewOwner();
            this.mUILayer.AddChild(rectTransform);
        }

        public override IEnumerator LoadViewOwnerAsync()
        {
            yield return base.LoadViewOwnerAsync();
            this.mUILayer.AddChild(rectTransform);
        }

        public override void UnloadViewOwner()
        {
            base.UnloadViewOwner();
            this.mUILayer.RemoveChild(rectTransform);
        }


    }
}