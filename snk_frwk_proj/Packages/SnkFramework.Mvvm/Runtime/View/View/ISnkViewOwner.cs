namespace SnkFramework.Mvvm.View
{
    public interface ISnkViewOwner : System.IDisposable
    {
        public bool mInteractable { get; set; }
    }
}