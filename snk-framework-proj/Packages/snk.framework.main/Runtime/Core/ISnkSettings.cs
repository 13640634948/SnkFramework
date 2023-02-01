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

        public class SnkSettings : ISnkSettings
        {
            public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }
            public bool ShouldRaisePropertyChanging { get; set; }
            public bool ShouldLogInpc { get; set; }
            public SnkSettings()
            {
                AlwaysRaiseInpcOnUserInterfaceThread = true;
                ShouldRaisePropertyChanging = true;
                ShouldLogInpc = false;
            }
            
        }
    }
}