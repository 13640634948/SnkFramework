using System.Collections.Generic;
using System.Threading.Tasks;

namespace snk.framework.nuget
{
    namespace patch
    {
        public interface ISnkRemotePatchRepository : ISnkPatchRepository
        {
            IEnumerable<SnkVersionMeta> GetResVersionHistories();

            Task TakeFileToLocal(string dirPath, string key, int resVersion);  
        } 
    }
}