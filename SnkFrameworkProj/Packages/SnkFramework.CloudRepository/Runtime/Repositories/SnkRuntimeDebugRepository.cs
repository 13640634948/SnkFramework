using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Repository
    {
        public class SnkRuntimeDebugRepository : SnkRepository<SnkLocalPersistentStorage, SnkRemoteBucketStorage>
        {

        }
    }
}