namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkRemoteStorage : SnkStorage, ISnkRemoteStorage
    {
        private SnkRemoteStorageSettings _settings;

        protected SnkRemoteStorage(SnkRemoteStorageSettings settings)
        {
            this._settings = settings;
        }
    }
}