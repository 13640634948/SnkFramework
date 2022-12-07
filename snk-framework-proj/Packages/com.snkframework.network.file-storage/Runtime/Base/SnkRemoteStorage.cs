using System;

namespace SnkFramework.Network.FileStorage
{
    namespace Runtime.Base
    {
        public abstract class SnkRemoteStorage : SnkStorage, ISnkRemoteStorage
        {
            public virtual string mEndPoint => throw new NotImplementedException();
            public virtual string mAccessKeyId => throw new NotImplementedException();
            public virtual string mAccessKeySecret => throw new NotImplementedException();
            public virtual string mBucketName => throw new NotImplementedException();
            protected virtual bool mIsQuietDelete => true;

        }
    }
}