using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    public class SnkCloudRepository : ISnkCloudRepository
    {
        protected ISnkStorage fromStorage { get; }
        protected ISnkStorage toStorage { get; }
        
        internal SnkCloudRepository(ISnkStorage fromStorage, ISnkStorage toStorage)
        {
            this.fromStorage = fromStorage;
            this.toStorage = toStorage;
        }

        public bool Preview(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> PreviewAsync(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Apply(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ApplyAsync(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
