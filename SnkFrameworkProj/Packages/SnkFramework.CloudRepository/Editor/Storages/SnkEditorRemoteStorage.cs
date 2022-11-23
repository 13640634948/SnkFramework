using SnkFramework.CloudRepository.Editor.Settings;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Editor.Storage
{
    public class SnkEditorRemoteStorage<T> : SnkRuntimeRemoteStorage
        where T : SnkStorageSettings,new()
    {
        protected readonly T _settings = SnkStorageSettings.Load<T>();
        public override string mEndPoint => _settings.mBucketName;
        public override string mAccessKeyId =>  _settings.mAccessKeyId;
        public override string mAccessKeySecret =>  _settings.mAccessKeySecret;
        public override string mBucketName => _settings.mBucketName;
        protected override bool mIsQuietDelete =>  _settings.mIsQuietDelete;
    }
}