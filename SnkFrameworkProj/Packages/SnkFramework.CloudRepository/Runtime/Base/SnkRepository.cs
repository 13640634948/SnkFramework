namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkRepository<TLocalStorage, TRemoteStorage>
        where TLocalStorage : class, ISnkLocalStorage
        where TRemoteStorage : class, ISnkRemoteStorage
    {
    }
}