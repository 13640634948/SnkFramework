using VContainer;

namespace SnkFramework.Runtime.Core.Setup
{
    public interface ISnkSetup
    {
        void InitializePrimary(IContainerBuilder builder);
        void InitializeSecondary(IContainerBuilder builder);
    }
}