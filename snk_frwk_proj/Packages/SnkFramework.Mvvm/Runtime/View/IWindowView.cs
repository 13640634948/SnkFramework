namespace View
{
    public interface IWindowView : UIContainer
    {
        public UIAnimation ActivationAnimation { get; }
        public UIAnimation PassivationAnimation { get; }
    }
}