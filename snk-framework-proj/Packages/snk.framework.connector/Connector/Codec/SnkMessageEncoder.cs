using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkMessageEncoder : ISnkMessageEncoder
    {
        public async Task<IEnumerable<byte>> Encode(ISnkMessage message)
        {
            return default;
        }
    }
}