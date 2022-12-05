using System.Threading.Tasks;
using SnkFramework.PatchBuilder.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkPatchStorage
    {
        public int Version { get; }
        public Task Initialize();

        public bool Exist(string sourceKey);
        public SnkSourceInfo GetSourceInfo(string key);
    }
}