namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public enum NavigationMode
        {
            None,
            Show,
            Close
        }
        
        public interface ISnkNavigateEventArgs
        {        bool Cancel { get; set; }
            NavigationMode Mode { get; set; }
            ISnkViewModel ViewModel { get; set; }
        }
    }
}