namespace Loxodon.Framework.Contexts
{
    public class Context
    {
        private static IServiceContainerProxy _serviceContainerProxy;
        public static IServiceContainerProxy GetApplicationContext()
            => _serviceContainerProxy ??= new ServiceContainerProxy();
    }
}