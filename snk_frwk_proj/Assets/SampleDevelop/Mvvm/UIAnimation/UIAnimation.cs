using System;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public abstract class UIAnimation : MonoBehaviour, IAnimation
    {
        private Action _onStart;
        private Action _onEnd;

        [SerializeField]
        private ANIM_TYPE animType;
        public ANIM_TYPE AnimType
        {
            get => this.animType;
            set => this.animType = value;
        }

        protected void OnStart()
        {
            try
            {
                this._onStart.Invoke();
                this._onStart = null;
            }
            catch (Exception) { }
        }

        protected void OnEnd()
        {
            try
            {
                this._onEnd?.Invoke();
                this._onEnd = null;
            }
            catch (Exception) { }
        }

        public IAnimation OnStart(Action onStart)
        {
            this._onStart += onStart;
            return this;
        }

        public IAnimation OnEnd(Action onEnd)
        {
            this._onEnd += onEnd;
            return this;
        }

        public abstract IAnimation Play();
    }
}