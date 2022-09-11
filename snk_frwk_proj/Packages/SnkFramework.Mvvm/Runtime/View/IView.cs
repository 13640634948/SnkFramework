namespace View
{
    public interface IView
    {
        public string mName { get; }
        public UIVector2 mPosition { get; }
        public UIVector2 mSize { get; }
        public bool mActivated { get; }

        public bool mInteractable { get; }
        public UIAnimation mEnterAnimation { get; }
        public UIAnimation mExitAnimation { get; }

        public UIAttribute[] mUIAttributes { get; }

    }
}