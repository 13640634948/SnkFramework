using System.Threading.Tasks;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkLocalSourceRepository : ISnkSourceRepository
    {
        public string LocalPath { get; }
    }
}