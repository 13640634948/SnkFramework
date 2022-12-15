using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkSourceRepository
    {
        public int Version { get; }
        public Task Initialize(SnkPatchSettings settings); 
        public Task<List<SnkSourceInfo>> GetSourceInfoList(int version=-1);
    }
}