using System;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    public class SnkCloudRepositoryFactory
    {
        public static ISnkCloudRepository Create<TFromStorage, TToStorage>( )
            where TFromStorage : class, ISnkStorage,new()
            where TToStorage : class, ISnkStorage,new()
        {
            if (typeof(TFromStorage) == typeof(TToStorage))
                throw new ArgumentException("TFromStorage and TToStorage is Same");

            var fromStorage = new TFromStorage();
            var toStorage = new TToStorage();
            return new SnkCloudRepository(fromStorage, toStorage);
        }
    }
}