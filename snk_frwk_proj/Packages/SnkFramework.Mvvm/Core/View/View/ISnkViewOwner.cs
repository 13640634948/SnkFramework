namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkViewOwner : System.IDisposable
        {
            public bool mInteractable { get; set; }
        }
    }
}