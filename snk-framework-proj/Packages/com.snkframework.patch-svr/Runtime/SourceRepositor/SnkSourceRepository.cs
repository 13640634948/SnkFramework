using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public abstract class SnkSourceRepository : ISnkSourceRepository
    {
        public abstract int Version { get; }
        protected SnkPatchSettings _settings { get; private set; }

        public virtual async Task Initialize(SnkPatchSettings settings)
        {
            this._settings = settings;
        }

        public abstract Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1);
    }
}