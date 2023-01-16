using System.Collections.Generic;
using System.Threading.Tasks;

namespace snk.framework.nuget
{
    namespace patch
    {
        public class SnkLocalPatchRepository : ISnkLocalPatchRepository
        {
            public int Version { get; }

            public Task Initialize()
            {
                throw new System.NotImplementedException();
            }

            public Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1)
            {
                throw new System.NotImplementedException();
            }

            public string LocalPath { get; }

            public void UpdateLocalResVersion(int resVersion)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}