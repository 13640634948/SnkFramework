namespace Loxodon.Framework.Contexts
{
    public interface IServiceContainerProxy
    {
        public T GetService<T>();
    }
}