using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public interface ISnkMessageEncoder
    {
        Task<IEnumerable<byte>> Encoder(ISnkMessage message);
    }
}