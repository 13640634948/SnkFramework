namespace SnkFramework.Runtime
{
    namespace Core
    {
        public interface ISnkSettings
        {
            bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

            bool ShouldRaisePropertyChanging { get; set; }

            bool ShouldLogInpc { get; set; }
        }
    }
}