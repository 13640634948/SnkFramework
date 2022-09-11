namespace View
{
    public abstract class WindowView<T> : View<T>, IWindowView
    {
        public abstract UIAnimation ActivationAnimation { get; }
        public abstract UIAnimation PassivationAnimation { get; }
    }
}