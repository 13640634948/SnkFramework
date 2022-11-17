using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Repository
    {
        public class SnkRuntimeAlphaRepository : SnkRepository<SnkLocalPersistentStorage, SnkRemoteIntranetStorage>
        {
        }
    }
}