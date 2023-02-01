namespace SnkFramework.Runtime
{
    namespace Core
    {
        public interface ISnkSetup
        {
            void InitializePrimary();
            void InitializeSecondary();
        }
    }
}