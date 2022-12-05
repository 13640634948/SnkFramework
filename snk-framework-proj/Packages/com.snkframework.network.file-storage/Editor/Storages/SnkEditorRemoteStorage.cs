using SnkFramework.CloudRepository.Editor.Settings;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Editor.Storage
{
    public class SnkEditorRemoteStorage<T> : SnkRuntimeRemoteStorage
        where T : SnkStorageSettings,new()
    {
        protected readonly T settings = SnkStorageSettings.Load<T>();
        public override string mBucketName => settings.mBucketName;
        public override string mEndPoint => settings.mEndPoint;
        public override string mAccessKeyId =>  settings.mAccessKeyId;
        public override string mAccessKeySecret =>  settings.mAccessKeySecret;
        protected override bool mIsQuietDelete =>  settings.mIsQuietDelete;
    }
}