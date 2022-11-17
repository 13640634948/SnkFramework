namespace SnkFramework.CloudRepository.Runtime
{
    public interface ISnkStorageValueOf<T>
    {
        public T StorageValueOf(object content);
    }
}