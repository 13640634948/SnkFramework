using System.Collections.Generic;
using SnkFramework.Network.FileStorage.Runtime.Base;

namespace SnkFramework.Network.FileStorage
{
    namespace Editor
    {
        public abstract class SnkEditorStorageBase<T> : SnkStorage
            where T : SnkStorageSettings, new()
        {
            protected readonly T settings = SnkStorageSettings.Load<T>();
            public virtual string mBucketName => settings.mBucketName;
            public virtual string mEndPoint => settings.mEndPoint;
            public virtual string mAccessKeyId => settings.mAccessKeyId;
            public virtual string mAccessKeySecret => settings.mAccessKeySecret;
            protected virtual bool mIsQuietDelete => settings.mIsQuietDelete;
          
        }
    }
}