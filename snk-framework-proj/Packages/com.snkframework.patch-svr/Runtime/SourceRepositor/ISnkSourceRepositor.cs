using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkSourceRepository
    {
        public int Version { get; }
        public Task Initialize();

        public bool Exist(string sourceKey);
        public SnkSourceInfo GetSourceInfo(string key);
    }
}