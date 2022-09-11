namespace View
{
    public abstract class View<T> : IView
    {
        public abstract string mName { get; }
        public abstract UIVector2 mPosition { get; }
        public abstract UIVector2 mSize { get; }
        public abstract bool mActivated { get; }
        public abstract bool mInteractable { get; }
        public abstract UIAnimation mEnterAnimation { get; }
        public abstract UIAnimation mExitAnimation { get; }
        public abstract UIAttribute[] mUIAttributes { get; }

        public T mOwner { get; }
        public T mParent { get; }

        protected virtual void OnVisibilityChanged() { }
    }
}