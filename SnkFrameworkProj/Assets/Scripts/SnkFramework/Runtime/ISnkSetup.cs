using VContainer;

namespace SnkFramework.Runtime
{
    public interface ISnkSetup
    {
        void InitializePrimary(IContainerBuilder builder);
        void InitializeSecondary(IContainerBuilder builder);
    }
}