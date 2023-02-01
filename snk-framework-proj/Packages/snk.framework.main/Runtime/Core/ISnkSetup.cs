namespace SnkFramework.Runtime.Core.Setup
{
    public interface ISnkSetup
    {
        void InitializePrimary();
        void InitializeSecondary();
    }
}