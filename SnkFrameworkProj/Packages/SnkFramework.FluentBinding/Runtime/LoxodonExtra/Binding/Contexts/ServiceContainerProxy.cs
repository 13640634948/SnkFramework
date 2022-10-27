using SnkFramework.FluentBinding.Base;

namespace Loxodon.Framework.Contexts
{
    public class ServiceContainerProxy : IServiceContainerProxy
    {
        private SnkIoCProvider _ioc;

        public ServiceContainerProxy()
            => _ioc = SnkIoCProvider.Instance;

        public T GetService<T>() => _ioc.Resolve<T>();
    }
}